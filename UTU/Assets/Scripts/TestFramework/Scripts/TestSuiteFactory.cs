

public class TestSuiteFactory {

    #region CONSTRUCTOR

    public TestSuiteFactory () {
        m_scriptsList = new System.Collections.Generic.List<string>();
    }

    #endregion

    #region PUBLIC

    public string GetFolderPath (string folderName) {
        var directories = System.IO.Directory.GetDirectories(UnityEngine.Application.dataPath, folderName, System.IO.SearchOption.AllDirectories);
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

        for (int i = 0; i < functions.Count; i++) {
            string[] args = new string[1] { functions[i] };
            var testCase = System.Activator.CreateInstance(System.Type.GetType(className), args) as TestCase;
            suite.Add(testCase);
        }
        return suite;
    }


    public TestSuite[] GetSuites (string folderName) {
        var path = GetFolderPath(folderName);
        var files = GetTestFilesAt(path);

        TestSuite[] suites = new TestSuite[files.Count];
        for (int i = 0; i < files.Count; i++) {
            suites[i] = GenerateSuite(files[i]);
        }

        return suites;
    }

    #endregion

    #region PRIVATE

    System.Collections.Generic.List<string> m_scriptsList;
    const string TEST_TAG = "Test";

    #endregion
}
