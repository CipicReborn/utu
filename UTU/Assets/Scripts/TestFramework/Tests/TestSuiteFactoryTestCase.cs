
public class TestSuiteFactoryTestCase : TestCase {

    #region CONSTRUCTOR

    public TestSuiteFactoryTestCase (string methodName) : base (methodName) {

    }

    #endregion


    #region PUBLIC

    public override void Setup () {
        base.Setup();
        m_suiteFactory = new TestSuiteFactory();
    }

    public void TestFindFolderPath () {
        var path = m_suiteFactory.GetFolderPath("tata");
        Assert(path == "");
        path = m_suiteFactory.GetFolderPath("DummyTestsFolder");
        Assert(path == UnityEngine.Application.dataPath + "\\Scripts\\TestFramework\\DummyTestsFolder");    
    }

    public void TestDetectingTestsFolder () {
        var path = m_suiteFactory.GetFolderPath("DummyTestsFolder");
        var paths = m_suiteFactory.GetTestFilesAt(path);
        Assert(paths.Count == 1);
        Assert(paths[0] == UnityEngine.Application.dataPath + "\\Scripts\\TestFramework\\DummyTestsFolder\\DummyTestCase.cs");
    }


    public void TestReadFunctionsInFile () {
        var file = UnityEngine.Application.dataPath + "\\Scripts\\TestFramework\\Tests\\DummyTestCaseTest.cs";
        var functions = m_suiteFactory.GetTestFunctionsNames(file, "DummyTestCaseTest");
        Assert(functions.Count == 10);
        Assert(functions[0] == "TestTestingPhases");
    }

    public void TestGenerateSuite () {
        var file = UnityEngine.Application.dataPath + "\\Scripts\\TestFramework\\Tests\\DummyTestCaseTest.cs";
        var suite = m_suiteFactory.GenerateSuite(file);
        Assert(suite.GetName() == "DummyTestCaseTest Suite");
        Assert(suite.Count == 10);
    }

    public void TestSuiteExecution () {
        var file = UnityEngine.Application.dataPath + "\\Scripts\\TestFramework\\Tests\\DummyTestCaseTest.cs";
        var suite = m_suiteFactory.GenerateSuite(file);
        var result = new TestResult();
        result = suite.Run(result);

        var witnessSuite = new TestSuite ("DummyTestCaseTest WitnessSuite");
        var witnessResult = new TestResult();
        witnessSuite.Add(new DummyTestCaseTest("TestTestingPhases"));
        witnessSuite.Add(new DummyTestCaseTest("TestPassingTestsAccounting"));
        witnessSuite.Add(new DummyTestCaseTest("TestFailingTestsAccounting"));
        witnessSuite.Add(new DummyTestCaseTest("TestDummyTestResultSummaryFormatting"));
        witnessSuite.Add(new DummyTestCaseTest("TestActualTestResultSummaryFormatting"));
        witnessSuite.Add(new DummyTestCaseTest("TestTestResultFailedSetupSummaryFormatting"));
        witnessSuite.Add(new DummyTestCaseTest("TestBrokenSetupDetection"));
        witnessSuite.Add(new DummyTestCaseTest("TestTestingPhasesWhenSetupFails"));
        witnessSuite.Add(new DummyTestCaseTest("TestTestSuiteSummary"));
        witnessSuite.Add(new DummyTestCaseTest("TestTestSuiteDetails"));
        witnessSuite.Run(witnessResult);

        Assert(result.GetTotalTestsCount() == witnessResult.GetTotalTestsCount());
        Assert(result.GetPassingTestsCount() == witnessResult.GetPassingTestsCount());
        Assert(result.GetDetails().Count == suite.Count);
        for (int i = 0; i < suite.Count; i++) {
            Assert(result.GetDetails()[i] == witnessResult.GetDetails()[i]);
        }
    }
    #endregion


    #region PRIVATE

    TestSuiteFactory m_suiteFactory;

    #endregion
}
