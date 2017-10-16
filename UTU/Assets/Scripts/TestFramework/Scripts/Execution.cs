using UnityEngine;

public class Execution : MonoBehaviour {

    #region PUBLIC

    public string GetFolderPath (string folderName) {
        var directories = System.IO.Directory.GetDirectories(Application.dataPath, folderName, System.IO.SearchOption.AllDirectories);
        if (directories.Length > 0) {
            return directories[0];
        }
        else {
            return "";
        }
    }

    public System.Collections.Generic.List<string> GetTestFilesAt (string folderPath) {
        m_scriptsList = new System.Collections.Generic.List<string>();
        if (System.IO.Directory.Exists(folderPath)) {
            var filePaths = System.IO.Directory.GetFiles(folderPath);
            for (int i = 0; i < filePaths.Length; i++) {
                var split = filePaths[i].Split('\\');
                var fileName = split[split.Length - 1];
                if (fileName.Contains(TEST_TAG) && !fileName.Contains(".meta")) {
                    m_scriptsList.Add(filePaths[i]);
                }
            }
        }
        return m_scriptsList;
    }

    public string GetClassNameFromFile (string filePath) {
        var lines = System.IO.File.ReadAllLines(filePath);
        for (int i = 0; i < lines.Length; i++) {
            var line = lines[i];
            if (line.Contains("class")) {
                return GetClassName(line);
            }
        }
        return "";
    }
    
    public System.Collections.Generic.List<string> GetTestFunctionsNames (string filePath, string className) {
        var functions = new System.Collections.Generic.List<string>();
        var lines = System.IO.File.ReadAllLines(filePath);
        for (int i = 0; i < lines.Length; i++) {
            var line = lines[i];
            if (line.Contains("public") && !line.Contains(className) && line.Contains(TEST_TAG)) {
                functions.Add(GetFunctionName(line));
            }
        }
        return functions;
    }

    string GetClassName (string line) {
        return GetWordAfter(line, "class");
    }

    string GetFunctionName (string line) {
        return GetWordAfter(line, "void");
    }

    string GetWordAfter (string line, string word) {
        var split = line.Split(' ');
        var i = System.Array.IndexOf(split, word);
        if (split.Length > i + 1) {
            return split[i + 1];
        }
        else {
            return "";
        }
    }

    public TestSuite GenerateSuite (string filepath) {
        var className = GetClassNameFromFile(filepath);
        var functions = GetTestFunctionsNames(filepath, className);
        var suite = new TestSuite(className + " Suite");
        System.Activator.CreateInstance(System.Type.GetType(className));
        for (int i = 0; i < functions.Count; i++) {

            suite.Add(new ExecutionTestCase(functions[i]));
        }
        return suite;
    }
    #endregion


    #region PRIVATE

    System.Collections.Generic.List<string> m_scriptsList;
    const string TEST_TAG = "Test";

    private void Awake () {
        m_scriptsList = new System.Collections.Generic.List<string>();
    }

    void Start () {
        
        RunFrameworkTests();
        RunSceneTests();
        RunExecutionTests();
    }

    void RunFrameworkTests () {
        var suite = new TestSuite("CheckupTestSuite");
        var result = new TestResult();

        suite.Add(new DummyTestCaseTest("TestTestingPhases"));
        suite.Add(new DummyTestCaseTest("TestPassingTestsAccounting"));
        suite.Add(new DummyTestCaseTest("TestFailingTestsAccounting"));
        suite.Add(new DummyTestCaseTest("TestDummyTestResultSummaryFormatting"));
        suite.Add(new DummyTestCaseTest("TestActualTestResultSummaryFormatting"));
        suite.Add(new DummyTestCaseTest("TestTestResultFailedSetupSummaryFormatting"));
        suite.Add(new DummyTestCaseTest("TestBrokenSetupDetection"));
        suite.Add(new DummyTestCaseTest("TestTestingPhasesWhenSetupFails"));
        suite.Add(new DummyTestCaseTest("TestTestSuiteSummary"));
        suite.Add(new DummyTestCaseTest("TestTestSuiteDetails"));

        suite.Run(result);

        PrintResults(result);
    }

    void RunSceneTests () {
        var suite = new TestSuite("Scene Setup Test Suite");
        var results = new TestResult();

        suite.Add(new SceneSetupTestCase("TestTestRunnerObjectSetup"));

        suite.Run(results);

        PrintResults(results);
    }

    void RunExecutionTests () {
        var suite = new TestSuite("Execution Test Suite");
        var results = new TestResult();

        suite.Add(new ExecutionTestCase("TestFindFolderPath"));
        suite.Add(new ExecutionTestCase("TestDetectingTestsFolder"));
        suite.Add(new ExecutionTestCase("TestReadFunctionsInFile"));
        suite.Add(new ExecutionTestCase("TestGenerateSuite"));
        
        suite.Run(results);

        PrintResults(results, true);
    }

    void PrintResults (TestResult result, bool showDetails = false) {
        Debug.Log(result.Summary());
        if (showDetails) {
            result.GetDetails().ForEach(args => { Debug.Log(args); });
        }
    }
    #endregion
    
}
