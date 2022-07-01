// testStaticLib.h : Header file for your target.

#pragma once

#pragma once

namespace MathLibrary
{
    class Arithmetic
    {
    public:
        // returns the sum of every integer from zero to maxNumber
        static long long ParallelSum(unsigned int maxNumber = 1000000000);
        // Returns a + b
        static double Add(double a, double b);

        // Returns a - b
        static double Subtract(double a, double b);

        // Returns a * b
        static double Multiply(double a, double b);

        // Returns a / b
        static double Divide(double a, double b);
    };
}