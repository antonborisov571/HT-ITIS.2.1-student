.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator
{
	.method static public float64 Calculate(float64 value1, valuetype Hw2.CalculatorOperation operation, float64 value2)
	{
		ldarg.1
		switch (Plus, Minus)
		
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
		
		Ret:
		ret
	}
}



