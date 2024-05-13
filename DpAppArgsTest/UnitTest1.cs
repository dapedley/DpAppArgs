namespace DpAppArgsTest;

[TestClass]
public class UnitTest1
{
	class Tst
	{
		public string [] Inp;
		public List<DpStringKvp> Exp;
		public bool Res;
		public Tst (string [] inp, List<DpStringKvp> exp, bool res)
		{ Inp = inp; Exp = exp; Res = res; }
	}

	[TestMethod]
	public void TestMethod1A ()
	{
		Tst tst = new Tst(
			new string [] { },
			new List<DpStringKvp> () {},
			true
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1B ()
	{
		Tst tst = new Tst(
				new string [] {"d:\\temp\\test.log" },
				new List<DpStringKvp> () {
					new DpStringKvp ("", "d:\\temp\\test.log")
				},
				true
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1C ()
	{
		Tst tst = new Tst(
				new string [] {"d:\\temp\\test.log", "test=xyz", "test2=abc" },
				new List<DpStringKvp> () {
					new DpStringKvp ("", "d:\\temp\\test.log"),
					new DpStringKvp ("test", "xyz"),
					new DpStringKvp ("test2", "abc")
				},
				true
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1D ()
	{
		Tst tst = new Tst(
				new string [] {"test=xyz", "test2=abc" },
				new List<DpStringKvp> () {
					new DpStringKvp ("test", "xyz"),
					new DpStringKvp ("test2", "abc")
				},
				true
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1E ()
	{
		Tst tst = new Tst(
				new string [] {"test=xyz", "unnamedArg", "test2=abc" },
				new List<DpStringKvp> () {
					new DpStringKvp ("test", "xyz"),
					new DpStringKvp (">", "Argument without a name - 'unnamedArg'"),
					new DpStringKvp ("test2", "abc")
				},
				false
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1F ()
	{
		Tst tst = new Tst(
				new string [] {"test=xyz", "=unnamedArg", "test2=abc" },
				new List<DpStringKvp> () {
					new DpStringKvp ("test", "xyz"),
					new DpStringKvp (">", "Invalid argument - '=unnamedArg'"),
					new DpStringKvp ("test2", "abc")
				},
				false
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1G ()
	{
		Tst tst = new Tst(
				new string [] {"test=xyz", "unnamedArg=", "test2=abc" },
				new List<DpStringKvp> () {
					new DpStringKvp ("test", "xyz"),
					new DpStringKvp ("unnamedarg", ""),
					new DpStringKvp ("test2", "abc")
				},
				true
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1H ()
	{
		Tst tst = new Tst(
				new string [] {"test", "-K", "test2=abc" },
				new List<DpStringKvp> () {
					new DpStringKvp ("", "test"),
					new DpStringKvp ("k", "-"),
					new DpStringKvp ("test2", "abc")
				},
				true
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1I ()
	{
		Tst tst = new Tst(
				new string [] {"test", "test2=abc", "-flag" },
				new List<DpStringKvp> () {
					new DpStringKvp ("", "test"),
					new DpStringKvp ("test2", "abc"),
					new DpStringKvp ("flag", "-")
				},
				true
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1J ()
	{
		Tst tst = new Tst(
				new string [] {"-flag", "test", "unnamedArg=", "test2=abc" },
				new List<DpStringKvp> () {
					new DpStringKvp ("flag", "-"),
					new DpStringKvp (">", "Argument without a name - 'test'"),
					new DpStringKvp ("unnamedarg", ""),
					new DpStringKvp ("test2", "abc")
				},
				false
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1K ()
	{
		Tst tst = new Tst(
				new string [] {"test", "-flag", "xx", "unnamedArg=", "test2=abc" },
				new List<DpStringKvp> () {
					new DpStringKvp ("", "test"),
					new DpStringKvp ("flag", "-"),
					new DpStringKvp (">", "Argument without a name - 'xx'"),
					new DpStringKvp ("unnamedarg", ""),
					new DpStringKvp ("test2", "abc")
				},
				false
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1L ()
	{
		Tst tst = new Tst(
				new string [] {"test", "test2=abc", "-i" },
				new List<DpStringKvp> () {
					new DpStringKvp ("", "test"),
					new DpStringKvp ("test2", "abc"),
					new DpStringKvp ("i", "-"),
				},
				true
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1M ()
	{
		Tst tst = new Tst(
				new string [] {"test", "test2=abc", "-i" },
				new List<DpStringKvp> () {
					new DpStringKvp ("", "test"),
					new DpStringKvp ("test2", "abc"),
					new DpStringKvp ("i", "-"),
				},
				true
		);
		Test1Proc (tst);
	}

	[TestMethod]
	public void TestMethod1N ()
	{
		Tst tst = new Tst(
				new string [] {"test", "test2=abc", "-i" },
				new List<DpStringKvp> () {
					new DpStringKvp ("", "test"),
					new DpStringKvp ("test2", "abc"),
					new DpStringKvp ("i", "-"),
				},
				true
		);
		Test1Proc (tst);
	}


	private void Test1Proc (Tst tst)
	{
		DpAppArgs args = new DpAppArgs (tst.Inp);
		List<DpStringKvp>? act = args.ArgList;
		bool ok = !args.HaveErrors;
		Assert.AreEqual (tst.Res, ok);
		CollectionAssert.AreEqual (tst.Exp, act);
	}



	[TestMethod]
	public void TestMethod2 ()
	{
		string [] inp = new string [] {"test", "Test1=abc", "-x", "tesT2=xyz"};

		DpAppArgs args = new DpAppArgs (inp);
		bool ok = !args.HaveErrors;
		Assert.AreEqual (true, ok, $"T2 - Parms");
		bool have1 = args.HaveKey ("TEST1");
		string? str1 = args.LastValue;
		Assert.AreEqual (true, have1, $"T2 - Test1 Ok");
		Assert.AreEqual ("abc", str1, $"T2 - Test1 Value");

		args = new DpAppArgs (inp);
		bool have2 = args.HaveKey ("ATest");
		string? str2 = args.LastValue;
		Assert.AreEqual (false, have2, $"T2 - Test2 Ok");
		Assert.AreEqual (null, str2, $"T2 - Test2 Value");

		string? str3 = args.GetValue ("test2");
		Assert.AreEqual ("xyz", str3, $"T2 - Test3 - GetValue");

		string? str4 = args.GetValue ("Another");
		Assert.AreEqual (null, str4, $"T2 - Test4 - GetValue");

		string? str5 = args.GetValue ("x");
		Assert.AreEqual ("-", str5, $"T2 - Test5 - GetValue");
		bool have5 = args.HaveKey ("X");
		Assert.AreEqual (true, have5, $"T2 - Test5 Ok");
		string? str5a = args.LastValue;
		Assert.AreEqual ("-", str5a, $"T2 - Test5 Value");

		string? str6 = args.GetValue ("q");
		Assert.AreEqual (null, str6, $"T2 - Test6 - GetValue");
		bool have6 = args.HaveKey ("Q");
		Assert.AreEqual (false, have6, $"T2 - Test6 Ok");
		string? str6a = args.LastValue;
		Assert.AreEqual (null, str6a, $"T2 - Test6 Value");

	}


}
