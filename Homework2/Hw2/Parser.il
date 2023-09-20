.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser
{
	.method public static void ParseCalcArguments(
      string[] args,
      [out] float64& val1,
      [out] valuetype Hw2.CalculatorOperation& operation,
      [out] float64& val2
    ) cil managed
	{
		ldarg.0
		call bool Hw2.Parser::IsArgLengthSupported(string[])
		brfalse.s ArgumentCount
		
		ldarg.0
		ldc.i4.0
		ldelem.ref
		ldarg.1
		call bool [System.Runtime]System.Double::TryParse(string, float64&)
		brfalse.s ArgumentFirst
		
		ldarg.0
		ldc.i4.2
		ldelem.ref
		ldarg.3
		call bool [System.Runtime]System.Double::TryParse(string, float64&)
		brfalse.s ArgumentSecond
		
		ldarg.2
		ldarg.0
		ldc.i4.1
		ldelem.ref
		call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
		stind.i4  
		br.s Ret
		
		ArgumentFirst:
		ldstr "Incorrect first value entered"
		newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
		throw
		
		ArgumentSecond:
		ldstr "Incorrect second value entered"
		newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
		throw
		
		ArgumentCount:
		ldstr "Incorrect number of arguments entered"
		newobj instance void [System.Runtime]System.ArgumentException::.ctor(string)
		throw
		
		Ret:
		ret
		
		
	}
	
	.method private static bool IsArgLengthSupported(string[] args) cil managed
	{
		ldarg.0 
		ldlen
		conv.i4
		ldc.i4 3
		ceq
		ret
	}
	
	.method private static valuetype Hw2.CalculatorOperation ParseOperation(string arg) cil managed
	{
		ldarg.0
        ldstr "+"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s Plus
    
        ldarg.0
        ldstr "-"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s Minus
    
        ldarg.0
        ldstr "*"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s Multiply
    
        ldarg.0
        ldstr "/"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s Divide
		
		ldstr "Entered incorrectly operation"
		newobj instance void [System.Runtime]System.InvalidOperationException::.ctor(string)
		throw
		
		Plus:
		ldc.i4.0
		br.s Ret
		
		Minus:
		ldc.i4.1
		br.s Ret
		
		Multiply:
		ldc.i4.2
		br.s Ret
		
		Divide:
		ldc.i4.3
		br.s Ret
		
		Ret:
		ret
		
	}
}


