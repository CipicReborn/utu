using UnityEngine;

public class Execution : MonoBehaviour {

    #region PRIVATE

    void Start () {
        RunTestsInFolder("Tests");
    }

    void RunTestsInFolder (string folderName) {
        var factory = new TestSuiteFactory();
        var suites = factory.GetSuites(folderName);
        for (int i = 0; i < suites.Length; i++) {
            RunSuiteAndPrintResults(suites[i]);
        }
    }

    void RunSuiteAndPrintResults (TestSuite suite) {
        var result = new TestResult();
        suite.Run(result);
        if (result.GetPassingTestsCount() == result.GetTotalTestsCount()) {
            PrintResults(result);
        }
        else {
            PrintResults(result, true);
        }
    }

    void PrintResults (TestResult result, bool showDetails = false) {
        Debug.Log(result.Summary());
        if (showDetails) {
            result.GetDetails().ForEach(args => { Debug.Log(args); });
        }
    }
    #endregion
    
}
