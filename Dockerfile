# Stage 1: Generate the SSL certificate
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS certgen

WORKDIR /app

ARG CERT_PASSWORD

# Generate the SSL certificate using a bash script
RUN apt-get update && apt-get install -y openssl
RUN openssl req -x509 -newkey rsa:4096 -nodes -keyout movie_crew_cert.key -out movie_crew_cert.crt -subj "/CN=localhost" -days 365
RUN openssl pkcs12 -export -out movie_crew_cert.pfx -inkey movie_crew_cert.key -in movie_crew_cert.crt -password pass:$CERT_PASSWORD
RUN rm movie_crew_cert.key movie_crew_cert.crt

# Stage 2: Build the .NET app
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /app

# Copy your .NET solution and project files
COPY . .
# Restore dependencies and build the app
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Stage 3: Run the .NET app with the generated certificate
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

WORKDIR /app

ARG CERT_PASSWORD

# Copy the published files from the build stage
COPY --from=build /app/out .

# Copy the generated SSL certificate to the runtime image
COPY --from=certgen /app/movie_crew_cert.pfx .

# Set environment variables for the API
ENV ASPNETCORE_URLS=https://*:443
ENV ASPNETCORE_HTTPS_PORT=443
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$CERT_PASSWORD
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/movie_crew_cert.pfx

# Expose the ports that the API application listens on
EXPOSE 443

# Run the API when the container starts
ENTRYPOINT ["dotnet", "MovieCrew.API.dll"]
