// testCMake.cpp : Defines the entry point for the application.
//

#include "testCMake.h"
#include "testSharedLibrary.h"
#include "testStaticLib.h"

using namespace std;

int main()
{
	cout << "Hello CMake." << endl;
	//cout << MathLibrary::Arithmetic::Add(1, 2) << endl;
	//FooClass foo;
	//foo.setA(5);
	//std::cout << "FooClass::a is " << foo.getA() << std::endl;
	fooPublicFunction(1);
	return 0;
}
