// testStaticLib.cpp : Source file for your target.
//
// compile with: cl /c /EHsc testStaticLib.cpp
// post-build command: lib testStaticLib.obj

#include "testStaticLib.h"
#include <tbb/parallel_for.h>
#include <tbb/scalable_allocator.h>
#include <vector>
#include <iostream>
#include <atomic>

using namespace std;

namespace MathLibrary
{
    long long Arithmetic::ParallelSum(unsigned int maxNumber)
    {
        std::atomic<long long> sum;
        tbb::parallel_for(tbb::blocked_range<int>(0, maxNumber),
            [&](const tbb::blocked_range<int>& r) {
                for (auto cell = r.begin(); cell != r.end(); ++cell) {
                    sum += cell;
                }
            });
        vector<int, tbb::scalable_allocator<int>> myVector;
        for (int i = 0; i < 10000; ++i) myVector.emplace_back(i);
        cout << "Parallel sum: " << sum << endl;
        return sum;
    }
    double Arithmetic::Add(double a, double b)
    {
        return a + b;
    }

    double Arithmetic::Subtract(double a, double b)
    {
        return a - b;
    }

    double Arithmetic::Multiply(double a, double b)
    {
        return a * b;
    }

    double Arithmetic::Divide(double a, double b)
    {
        return a / b;
    }
}