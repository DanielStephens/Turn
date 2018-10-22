using UnityEngine;

public static class MonoBehaviourHolder {

	private static MonoBehaviour instance = null;

	// Use this for initialization
	public static MonoBehaviour MonoBehaviour () {
		if(instance == null){
			instance = buildMonoBehaviour();
		}
		return instance;
	}

	private static MonoBehaviour buildMonoBehaviour(){
		GameObject o = new GameObject("MONO_BEHAVIOUR_HOLDER");
		MonoBehaviour mb = o.AddComponent<MonoBehaviour>() as MonoBehaviour;
		return mb;
	}

}
