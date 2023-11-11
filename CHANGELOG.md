# Changelog

All notable changes to this project will be documented in this file. See [standard-version](https://github.com/conventional-changelog/standard-version) for commit guidelines.

## [1.1.0](https://github.com/MaximeMohandi/BillB0ard-API/compare/v1.0.0...v1.1.0) (2023-11-11)


### Features

* **security:** 🎸 add certificate authentication ([e777e6e](https://github.com/MaximeMohandi/BillB0ard-API/commit/e777e6e42c03e5e664bd0f0e5995ade50def693a))

## 1.0.0 (2023-08-16)


### ⚠ BREAKING CHANGES

* 🧨 Y
* 🧨 Y
* 🧨 N
* 🧨 N

### Features

* 🎸 add third party to inject in movie service ([2f32756](https://github.com/MaximeMohandi/BillB0ard-API/commit/2f327561534fb0506601e07ee83da8596850f110))
* 🎸 Add TMDB provider for movies data ([7951ed2](https://github.com/MaximeMohandi/BillB0ard-API/commit/7951ed20f742b83804bd24423882328af1d3be83))
* 🎸 added a route to register user ([3aa5169](https://github.com/MaximeMohandi/BillB0ard-API/commit/3aa51698f8cf2fc4e55019946a5ee8f1b4136147))
* 🎸 added client to database context ([fb25a6a](https://github.com/MaximeMohandi/BillB0ard-API/commit/fb25a6a8d4da1f9ada5fcc94f9df3bdab4e94291))
* 🎸 added exception when role do not exist ([b311934](https://github.com/MaximeMohandi/BillB0ard-API/commit/b31193462f07d6cc34eda9b2325c7e41abc956a3))
* 🎸 api configuration skeleton ([a4fce65](https://github.com/MaximeMohandi/BillB0ard-API/commit/a4fce65ac0eacebfdaa467a0df92a11cd898d29a))
* 🎸 can't register user with unknown role ([7b2669f](https://github.com/MaximeMohandi/BillB0ard-API/commit/7b2669f0803f23f653cd2d927213b74c8c7a6dff))
* 🎸 changed authentication to get method ([c2d6b34](https://github.com/MaximeMohandi/BillB0ard-API/commit/c2d6b347e2690ee4a8b0c9d11a152f720d054ff2))
* 🎸 controller auth with apikey instead of user ([4ca26fe](https://github.com/MaximeMohandi/BillB0ard-API/commit/4ca26fe5a356fa5dd25df6dddd5a0325575d9752))
* 🎸 domain authentication based on client instead of user ([67399d9](https://github.com/MaximeMohandi/BillB0ard-API/commit/67399d99bb1d0bed29f205dfc49e40c3e7b9f988))
* 🎸 fetch budget line for movie details ([c7af4e7](https://github.com/MaximeMohandi/BillB0ard-API/commit/c7af4e7129063a05858ca458067e64e6d8a779b3))
* 🎸 get third party movie base url from config ([2ee8390](https://github.com/MaximeMohandi/BillB0ard-API/commit/2ee8390a581d48ed3f0085f0ba5386f5cd3bef45))
* 🎸 login url ([74fe276](https://github.com/MaximeMohandi/BillB0ard-API/commit/74fe2764edc54a2ba55bf2e161d9d48ecb77cae3))
* 🎸 login user ([3ab8454](https://github.com/MaximeMohandi/BillB0ard-API/commit/3ab84549c39b717d36253b322a0f572f4f9bc2dd))
* 🎸 mapped db context with database ([e433ac6](https://github.com/MaximeMohandi/BillB0ard-API/commit/e433ac62001e79808cbd511ee159dfa1425fef56))
* 🎸 register an user with id ([12ca7c3](https://github.com/MaximeMohandi/BillB0ard-API/commit/12ca7c360abe37cd376189cc974126076eb67002))
* 🎸 rename movie route ([8b8023f](https://github.com/MaximeMohandi/BillB0ard-API/commit/8b8023f6d72529985a734c3c6140de89f9c7ddba))
* 🎸 secure user endpoint ([5ff0814](https://github.com/MaximeMohandi/BillB0ard-API/commit/5ff08141f7d68dde4b7f88ae7d5988b78fa8a320))
* 🎸 throw 400 when registering user that already exist ([98d2a5d](https://github.com/MaximeMohandi/BillB0ard-API/commit/98d2a5d0b2b0995c8787c5767425225d9e7f16de))
* 🎸 update poster ([54b9508](https://github.com/MaximeMohandi/BillB0ard-API/commit/54b9508c97ff7440f18ad229a6352daa006c6c8e))
* add bad request when rate is out of bound ([72e6fd9](https://github.com/MaximeMohandi/BillB0ard-API/commit/72e6fd9eb43d381a7e564dfa5b534418a540a77e))
* authentication controller ([d920e37](https://github.com/MaximeMohandi/BillB0ard-API/commit/d920e3767ba8536aa76f062760d3478f23e92d72))
* **authentication:** add an authentication in program.cs ([fc47ad4](https://github.com/MaximeMohandi/BillB0ard-API/commit/fc47ad47b2657849c1d506c0b4c7ac4a1f6ccd12))
* **authentication:** add authorization on routes ([6c9efda](https://github.com/MaximeMohandi/BillB0ard-API/commit/6c9efdac7e06782630142b5108ad23fb1d3b85c4))
* **authentication:** add protection on api/movie route ([ed70352](https://github.com/MaximeMohandi/BillB0ard-API/commit/ed70352d68ae50b30798863a086e25a1ca3afe07))
* **authentication:** add protection on api/rate route ([e5fc3c1](https://github.com/MaximeMohandi/BillB0ard-API/commit/e5fc3c1a1338c1e40998b3ea17d127b39ce55002))
* **authentication:** add protection to api/spectator route ([6a33517](https://github.com/MaximeMohandi/BillB0ard-API/commit/6a33517034d0048c3823e9b8e09454de7bd8ea9b))
* **class:** user exception handling with http error code ([8213b23](https://github.com/MaximeMohandi/BillB0ard-API/commit/8213b23a4c5b1aad6846a3b1ced5a739c20e8c6b))
* **program.cs:** inject dependance ([466410e](https://github.com/MaximeMohandi/BillB0ard-API/commit/466410e548bdb8e60613911fdacba09b86699084))
* rename movie ([65b6344](https://github.com/MaximeMohandi/BillB0ard-API/commit/65b6344779555680f9195e6473aaf0a17a5c18b3))


### Bug Fixes

*  import identiy model in core project ([19b8385](https://github.com/MaximeMohandi/BillB0ard-API/commit/19b83854f06911a6ec02944e83e6709906890a3c))
* 🐛 detailled movies return rate null if not rated ([407e7a1](https://github.com/MaximeMohandi/BillB0ard-API/commit/407e7a19efb003790726d69e89d2f81108dae44e))
* 🐛 fixed rates where not fetched for movies when get all ([541c488](https://github.com/MaximeMohandi/BillB0ard-API/commit/541c488b05e89617319adb5c4eb275508e028987))
* 🐛 get rates and movies entity from user ([e3f7933](https://github.com/MaximeMohandi/BillB0ard-API/commit/e3f7933806e4caa24e0529ed30632bfdd2d70c5c))
* 🐛 includes rates for detailed movies ([d9d809b](https://github.com/MaximeMohandi/BillB0ard-API/commit/d9d809b1ae2a6302928e7102c44181cecb5fa6f8))
* **authentication:** changed route from get to post ([f8e604e](https://github.com/MaximeMohandi/BillB0ard-API/commit/f8e604e6686b9368d919295564454f672fb2319c))
