

# This library is compiled as a DYNAMIC library by default, unless
# FOO_LIBRARY_STATIC is set to TRUE.
if (NOT DEFINED FOO_LIBRARY_STATIC)
	set(FOO_LIBRARY_STATIC FALSE)
endif()

# List all library sources
set(FOO_LIBRARY_SOURCES
	testSharedLibrary.cpp
	testSharedLibrary.h
	foo_support.h
)

# Create static or shared library target.
if (FOO_LIBRARY_STATIC)
	# Static
	add_library(testSharedLibrary STATIC ${FOO_LIBRARY_SOURCES})

	# Define FOO_LIBRARY_STATIC when building and using this library.
	target_compile_definitions(testSharedLibrary
		PUBLIC
			FOO_LIBRARY_STATIC
	)

else()
	# Dynamic
	add_library(testSharedLibrary SHARED ${FOO_LIBRARY_SOURCES})

	# Make all non-exported symbols are hidden by default (similar to Windows)
	set_target_properties(testSharedLibrary
		PROPERTIES
			C_VISIBILITY_PRESET hidden
			CXX_VISIBILITY_PRESET hidden
	)
endif()

target_link_libraries(testSharedLibrary 
	PUBLIC
        testStaticLib
)
# Treat the exported symbols as exported (and not imported) when building the library
target_compile_definitions(testSharedLibrary
	PRIVATE
		FOO_LIBRARY_BUILD
)

# Export the library's public include path to dependent targets
target_include_directories(testSharedLibrary
	INTERFACE
		${CMAKE_CURRENT_SOURCE_DIR}
)

#add_custom_target(install_SharedLib
   # Dont use cmake --install for compatibility
#   COMMAND ${CMAKE_COMMAND} cmake -DCOMPONENT=testSharedLibrary -P ${CMAKE_BINARY_DIR}/cmake_install.cmake
#)

add_custom_command(TARGET testSharedLibrary POST_BUILD 
  COMMAND "${CMAKE_COMMAND}" -E copy 
     "$<TARGET_FILE:testSharedLibrary>"
     "/mnt/c/Users/TroyBerg/source/repos/testWebApi2/native/out/linux/$<CONFIGURATION>/$<TARGET_FILE_NAME:testSharedLibrary>" 
  COMMENT "Copying testSharedLibrary to output directory")

  