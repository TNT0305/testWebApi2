# testCMake
Demontrating how to have a cross platform C++ application shared(or static) library and a static library.

The goal is to get it to the point that we can publish a docker image of a .NET 6 webApi that references a C++ library in a linux container.

# References
- docker and cmake: https://www.jmoisio.eu/en/blog/2020/06/01/building-cpp-containers-using-docker-and-cmake/
- cmake presets official documentation: https://cmake.org/cmake/help/latest/manual/cmake-presets.7.html
- shared/static
  - v1: ???
  - v2: https://alexreinking.com/blog/building-a-dual-shared-and-static-library-with-cmake.html
- nanoFramework build notes: https://www.nanoframework.net/build-updated-to-cmake-presets/

- multi-platform nuget example: https://stackoverflow.com/questions/69393627/create-nuget-package-for-different-platforms-architectures-and-net-versions