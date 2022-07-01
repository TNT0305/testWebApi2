// testSharedLibrary.cpp : Source file for your target.
//

#include "testSharedLibrary.h"
#include "testStaticLib.h"


void FooClass::setA(int value)
{
	a = value;
}



int FooClass::getA()
{
	return a;
}



int fooPublicFunction(int a)
{
	return a + 1;
}


FooClass* createFoo() { return new FooClass(); }
void disposeFoo(FooClass* instance) { 
	if (instance != nullptr) {
		delete instance;
		instance = nullptr;
	}
}

void fooSetA(FooClass* instance, int value)
{
	if (instance == nullptr) return;
	instance->setA(value);
}
int fooGetA(FooClass* instance)
{
	if (instance == nullptr) return 0;
	return instance->getA();
}
long long ParallelSum(unsigned int maxNumber) {
	return MathLibrary::Arithmetic::ParallelSum(maxNumber);
}