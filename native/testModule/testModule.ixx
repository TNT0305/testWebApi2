//export module $safeitemname$;
//
//export void MyFunc();


// Printers.ixx
// 
// The .ixx extension lets the build system know this file contains
// a module interface.

// Begin global module fragment.
module;

// Headers included in the global module fragment (between "module;" and
// "export module Printers;") can be used by the module implementation but
// are not exported. These included headers are invisible to translation
// units that import the Printers module.
#include <iostream>

// Creates the Printers module. This can be imported into other translation
// units with "import Printers;" Any items marked with the export keyword
// will be available in translation units that import the Printers module.
export module Printers;

// This only applies to this module's translation unit. It does not leak
// into translation units that import the Printers module.
using namespace std;

// These constants are not exported, they are invisible from translation
// units that import the Printer module.
const string default_spacer = " ";
const string default_seperator = ",\n";

// SimplePrinter is exported and accessible to any code that imports the
// Printers module.
export struct SimplePrinter
{
    string element_spacer;
    string seperator;

    void print_element(int e)
    {
        std::cout << e << element_spacer;
    }

    void print_seperator()
    {
        std::cout << seperator;
    }
};

// Exports the function get_default_printer.
// This is accessible from translation units that import the Printers module.
export SimplePrinter get_default_printer()
{
    //return SimplePrinter{default_spacer,::default_separator };
    return SimplePrinter{
        .element_spacer = default_spacer,
        .seperator = default_seperator
    };
}