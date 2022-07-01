# CMakeList.txt : Top-level CMake project file, do global configuration
# and include sub-projects here.
#
cmake_minimum_required (VERSION 3.8)

project ("testCMake")
# Foo library is dynamic by default. Introduce a switch to override that.
option(FOO_LIBRARY_STATIC "Compile foo_library as static library" OFF)

# Include sub-projects.
add_subdirectory ("testCMake")

add_subdirectory("testStaticLib")

#add_subdirectory("testModule")

add_subdirectory("testSharedLibrary")

