
add_library(testModule MODULE  "testModule.ixx")
if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET testModule PROPERTY CXX_STANDARD 20)
endif()

add_custom_command(
    TARGET testModule
    COMMAND echo CMAKE_VERSION
    )