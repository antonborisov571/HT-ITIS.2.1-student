.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator
{
	.method static public float64 Calculate(float64 value1, valuetype Hw2.CalculatorOperation operation, float64 value2)
	{
		ldarg.1
		switch (Plus, Minus, Multiply, Divide, Undefined)
		
		Plus:
		ldarg.0
		ldarg.2
		add
		br.s Ret
		
		Minus:
		ldarg.0
		ldarg.2
		sub
		br.s Ret
		
		Multiply:
		ldarg.0
		ldarg.2
		mul
		br.s Ret
		
		Divide:
		ldarg.0
		ldarg.2
		div
		br.s Ret
		
		Undefined:
		newobj instance void [System.Runtime]System.InvalidOperationException::.ctor()
		throw
		
		Ret:
		ret
	}
}



