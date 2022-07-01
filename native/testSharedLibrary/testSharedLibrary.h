/**************************************************************************
Copyright (C) 2021 Alexander Shaduri
License: 0BSD (Zero-Clause BSD)
***************************************************************************/
#ifndef FOO_LIBRARY_H
#define FOO_LIBRARY_H

#include "foo_support.h"

class FooClass {
public:
	// Avoid using inline functions to access class data members,
	// otherwise they will also be exported.
	void setA(int value);
	int getA();

private:
	int a = 0;
};

extern "C" {	// eliminate name mangling
	FOO_LIBRARY_EXPORT int fooPublicFunction(int a);

	////////////////////////
	// exports for "FooClass"
	// It is uncumbent upon the caller of createFoo to call disposeFoo.  Otherwise it is a memory leak. (Implement ISdisposable with the pattern)
	FOO_LIBRARY_EXPORT FooClass* createFoo();
	FOO_LIBRARY_EXPORT void disposeFoo(FooClass* instance);
	FOO_LIBRARY_EXPORT void fooSetA(FooClass* instance, int value);
	FOO_LIBRARY_EXPORT int fooGetA(FooClass* instance);

	FOO_LIBRARY_EXPORT long long ParallelSum(unsigned int maxNumber = 1000000000);

}
#endif
