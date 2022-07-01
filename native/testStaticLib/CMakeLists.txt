# List all library sources
set(STATIC_LIBRARY_SOURCES
	testStaticLib.cpp
	testStaticLib.h
)
set(CMAKE_POSITION_INDEPENDENT_CODE ON)
add_library(testStaticLib STATIC ${STATIC_LIBRARY_SOURCES})

find_package(TBB CONFIG REQUIRED)
target_link_libraries(testStaticLib PRIVATE TBB::tbb TBB::tbbmalloc)

# Export the library's public include path to dependent targets
target_include_directories(testStaticLib
	INTERFACE
		${CMAKE_CURRENT_SOURCE_DIR}
)