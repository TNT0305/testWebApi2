# CMakeList.txt : CMake project for testCMake, include source and define
# project specific logic here.
#
cmake_minimum_required (VERSION 3.8)

# Add source to this project's executable.
add_executable (testCMake "testCMake.cpp" "testCMake.h")

target_link_libraries(testCMake 
	PRIVATE
		testSharedLibrary
)
# include_directories("../testStaticLib")

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET testCMake PROPERTY CXX_STANDARD 20)
endif()

if (WIN32)
# copy the .dll file to the same folder as the executable
add_custom_command(
    TARGET testCMake POST_BUILD
    COMMAND ${CMAKE_COMMAND} -E copy_directory
    $<TARGET_FILE_DIR:testSharedLibrary>
    $<TARGET_FILE_DIR:testCMake>)
endif()
