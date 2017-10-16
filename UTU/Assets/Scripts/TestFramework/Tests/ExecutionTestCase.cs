
public class ExecutionTestCase : TestCase {

    #region CONSTRUCTOR

    public ExecutionTestCase (string methodName) : base (methodName) {

    }

    #endregion


    #region PUBLIC

    public override void Setup () {
        base.Setup();
        m_executionScript = UnityEngine.GameObject.Find("TestRunner").GetComponent<Execution>();
    }

    public void TestFindFolderPath () {
        var path = m_executionScript.GetFolderPath("tata");
        Assert(path == "");
        path = m_executionScript.GetFolderPath("DummyTestsFolder");
        Assert(path == "F:/Projects/U2T/U2T/Assets\\Scripts\\TestFramework\\DummyTestsFolder");
    }

    public void TestDetectingTestsFolder () {
        var path = m_executionScript.GetFolderPath("DummyTestsFolder");
        var paths = m_executionScript.GetTestFilesAt(path);
        Assert(paths.Count == 2);
        Assert(paths[0] == "F:/Projects/U2T/U2T/Assets\\Scripts\\TestFramework\\DummyTestsFolder\\DummyTestCase.cs");
        Assert(paths[1] == "F:/Projects/U2T/U2T/Assets\\Scripts\\TestFramework\\DummyTestsFolder\\DummyTestCaseTest.cs");
    }


    public void TestReadFunctionsInFile () {
        var file = "F:/Projects/U2T/U2T/Assets\\Scripts\\TestFramework\\DummyTestsFolder\\DummyTestCaseTest.cs";
        var functions = m_executionScript.GetTestFunctionsNames(file, "DummyTestCaseTest");
        Assert(functions.Count == 10);
        Assert(functions[0] == "TestTestingPhases");
    }

    public void TestGenerateSuite () {
        var file = "F:/Projects/U2T/U2T/Assets\\Scripts\\TestFramework\\DummyTestsFolder\\DummyTestCaseTest.cs";
        var suite = m_executionScript.GenerateSuite(file);
        Assert(suite.GetName() == "DummyTestCaseTest Suite");
        Assert(suite.Count == 10);
    }

    #endregion

    #region PRIVATE

    Execution m_executionScript;

    #endregion
}
