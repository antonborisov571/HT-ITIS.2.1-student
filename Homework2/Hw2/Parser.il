.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser
{
	.method private hidebysig static bool IsArgLengthSupported(string[] args) cil managed
	{
		ldarg.0 
		ldlen
		conv.i4
		ldc.i4 3
		ceq
		ret
	}
}


