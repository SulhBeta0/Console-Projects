//Finished on ??? ??th, 2025 -

/*
 * We can/have -
 * 1. Add & Substract (+/-) numbers.
 * 2. Multiply & divide (+/-) numbers.
 * 3. Arithmetic hierarchy.
 * 5. Use parentheses. (Partially finished. Sometimes it works)
 * 
 * Future updates:
 * 4. Complete the basic hierarchy (rasing to the power).
 * 6. Refactor all the code.
 * 7. Square rooting (i'll try).
 */

namespace PJ12_Text_Calculator;

public class Program
{
    static void Main()
    {
        Calculator calculator = new();
        calculator.Update();

        Environment.Exit(0);
    }
}