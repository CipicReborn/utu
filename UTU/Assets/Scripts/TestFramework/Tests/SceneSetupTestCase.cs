

public class SceneSetupTestCase : TestCase {

    #region CONSTRUCTOR

    public SceneSetupTestCase (string methodName) : base(methodName) { }

    #endregion


    #region PUBLIC

    public void TestTestRunnerObjectSetup () {
        var runner = UnityEngine.GameObject.Find("TestRunner");
        Assert(runner != null);
        Execution script = runner.GetComponent<Execution>();
        Assert(script != null);
        Assert(script.enabled);
    }

    #endregion


    #region PRIVATE




    #endregion
}
