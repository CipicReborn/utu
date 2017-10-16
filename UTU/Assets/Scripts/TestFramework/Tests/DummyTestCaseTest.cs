
public class DummyTestCaseTest : TestCase {

    public DummyTestCaseTest (string methodName): base(methodName) {}

    public override void Setup () {
        base.Setup();
        m_result = new TestResult();
    }

    //public override void Teardown() {
    //    base.Teardown();
    //    m_result = null;
    //}

    public void TestTestingPhases () {
        var dummyTestCase = new DummyTestCase("DummyPassingTest");
        dummyTestCase.Run(m_result);
        Assert(dummyTestCase.StateLog == "Setup Run Teardown");
    }

    public void TestPassingTestsAccounting () {
        var dummyTestCase = new DummyTestCase("DummyPassingTest");
        var result = dummyTestCase.Run(m_result);
        Assert(result.GetTotalTestsCount() == 1);
        Assert(result.GetPassingTestsCount() == 1);
    }

    public void TestFailingTestsAccounting () {
        var dummyTestCase = new DummyTestCase("DummyFailingTest");
        var result = dummyTestCase.Run(m_result);
        Assert(result.GetFailingTestsCount() == 1);
    }

    public void TestDummyTestResultSummaryFormatting () {
        TestResult result;
        result = new TestResult();
        result.SetTestName("Not Tested");
        Assert(result.Summary() == "Not Tested : 0 run, 0 failed");
        result = new TestResult();
        result.SetTestName("Passing Test");
        result.StartTest();
        Assert(result.Summary() == "Passing Test : 1 run, 0 failed");
        result = new TestResult();
        result.SetTestName("Failing Test");
        result.StartTest();
        result.TestFailed();
        Assert(result.Summary() == "Failing Test : 1 run, 1 failed");
    }

    public void TestActualTestResultSummaryFormatting () {
        var result = new TestResult();
        var dummyTestCase = new DummyTestCase("DummyPassingTest");
        dummyTestCase.Run(result);
        Assert(result.Summary() == "DummyPassingTest : 1 run, 0 failed");
    }

    public void TestTestResultFailedSetupSummaryFormatting () {
        var result = new TestResult() {
            IsSetupSuccessful = false
        };
        Assert(result.Summary() == " : [Error] Setup Failed. 0 run, 0 failed");
    }

    public void TestBrokenSetupDetection () {
        var dummyTestCase = new DummyTestCase("DummyPassingTest");
        dummyTestCase.SetSetupFail(true);
        var result = dummyTestCase.Run(m_result);
        Assert(!result.IsSetupSuccessful);
    }

    public void TestTestingPhasesWhenSetupFails () {
        var dummyTestCase = new DummyTestCase("DummyPassingTest");
        dummyTestCase.SetSetupFail(true);
        var result = dummyTestCase.Run(m_result);
        Assert(result.IsTeardownSuccessful);
        Assert(dummyTestCase.StateLog == "Teardown");
    }

    public void TestTestSuiteSummary () {
        var testSuite = new TestSuite("DummySuiteOfTwo");
        testSuite.Add(new DummyTestCase("DummyPassingTest"));
        testSuite.Add(new DummyTestCase("DummyFailingTest"));

        var result = new TestResult();
        testSuite.Run(result);

        Assert(result.Summary() == "DummySuiteOfTwo : 2 run, 1 failed");
    }

    public void TestTestSuiteDetails () {
        var testSuite = new TestSuite("DummySuiteOfTwo");
        testSuite.Add(new DummyTestCase("DummyPassingTest"));
        testSuite.Add(new DummyTestCase("DummyFailingTest"));

        var result = new TestResult();
        testSuite.Run(result);
        var list = result.GetDetails();
        
        Assert(list.Count == 2);
        Assert(list[0] == "DummyPassingTest : Passed");
        Assert(list[1] == "DummyFailingTest : Failed");
    }

    TestResult m_result = null;
}
