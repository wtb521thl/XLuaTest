function TestFunction(params)
	CS.UnityEngine.Debug.Log("Click"..(typeof(params)==nil and params[0] or ""))

	hue=CS.UnityEngine.Random.Range(0,1)

	image:GetComponent(typeof(CS.UnityEngine.UI.Image)).color=CS.UnityEngine.Color.HSVToRGB(hue,1,1)

	testObj=CS.UnityEngine.GameObject.CreatePrimitive(CS.UnityEngine.PrimitiveType.Cube)

	testObj.transform.position=CS.UnityEngine.Vector3(CS.UnityEngine.Random.Range(-10,10),CS.UnityEngine.Random.Range(-10,10),CS.UnityEngine.Random.Range(-10,10))
end