using System.Data;
using System.Runtime.CompilerServices;
using System.Text;


HashSet<char> Digits = new HashSet<char> { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0','.'};
HashSet<char> Operators = new HashSet<char> { '+', '-', '*', '/'};

Console.WriteLine("  CALCULATOR\n\n");

Console.WriteLine("You can write numbers and { + , - , * , / , . , ( , ) } ");
Console.WriteLine("Press Enter without writing to exit\n");

while (true)
{
    Console.Write(">> ");
    string? equation = Console.ReadLine();

    if(string.IsNullOrEmpty(equation) )
        Environment.Exit(0);

    var ans = Solve(equation);

    Console.WriteLine(">> "+ans);
    Console.WriteLine(new string('\u2500', 30));
}


# region Methods
string Solve(string? eq)
{
    if (string.IsNullOrEmpty(eq))
        return "Invalid Input";

    eq = eq.Replace(" ", string.Empty);
    for (int i = 0; i < eq.Length; i++)
    {
        if (!(Digits.Contains(eq[i]) || Operators.Contains(eq[i]) 
            || eq[i] == '(' || eq[i] == ')'))
            return "Invalid Input";
    }

    while (eq.Contains('(') || eq.Contains(')'))
    {
        var stack = new Stack<int>();

        for (int i = 0;i < eq.Length; i++)
        {
            if (eq[i] == '(')
            {
                stack.Push(i);
                if (!eq.Contains(')'))
                    return "Incomplete Parentheses";

            }
            else if (eq[i] == ')')
            {
                if (stack.Count == 0)
                    return "Incomplete Parentheses";

                int temp = stack.Pop();
                string? innerEq = eq.Substring(temp + 1, i - temp - 1);
                if (string.IsNullOrEmpty(innerEq))
                    return "Incomplete Parentheses";

                decimal? innerDecimal = Evaluate(innerEq);
                if (innerDecimal is null)
                    return "Invalid Input";
                innerEq = innerDecimal.ToString();

                string addMult = "";
                if (temp != 0 && !( Operators.Contains(eq[temp-1]) ))
                    addMult = "*";
                eq = eq.Substring(0, temp) + addMult + "0" + innerEq + eq.Substring(i+1);
            }
        }
    }

    decimal? res = Evaluate(eq);
    if (res is null)
        return "Invalid Input";
    return res.ToString() ?? "Invalid Input";
}
decimal? Evaluate(string eq)
{
    if (eq == null)
        return 0;
    if (eq[0] == '-')
        eq = "0" + eq;
    var numbers = new List<decimal>();
    var operators = new List<char>();
    var current = "";

    for (int j = 0; j < eq.Length; j++)
    {
        if (Digits.Contains(eq[j]))
            current += eq[j];
        else if (Operators.Contains(eq[j]))
        {
            if (!decimal.TryParse(current, out decimal n))
                return null;
            numbers.Add(n);
            operators.Add(eq[j]);
            current = "";
        }
    }
    if (!decimal.TryParse(current, out decimal m))
        return null;
    numbers.Add(m);

    if (numbers.Count != operators.Count + 1)
        return null;

    int i = 0;
    decimal result = 0;
    while (i < operators.Count)
    {
        if (operators[i] == '*' || operators[i] == '/')
        {
            if (operators[i] == '*')
                result = numbers[i] * numbers[i + 1];
            else
                result = numbers[i] / numbers[i + 1];

            numbers[i] = result;
            numbers.RemoveAt(i + 1);
            operators.RemoveAt(i);
        }
        else
            i++;
    }

    result = numbers[0];

    for (int j = 0; j < operators.Count ; j++)
    {
        if (operators[j] == '+')
            result += numbers[j + 1];
        else
            result -= numbers[j + 1];
        
    }

    return result;
}

#endregion