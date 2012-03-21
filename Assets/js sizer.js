#pragma strict
var scale: float=0.1;
var a:float;
function Start () {
}


function Update () {
	transform.localScale=Vector3(scale,0.2,scale);


	//these lines assume we have one of the axis imputting, and named theta
	//scale=Input.GetAxis("theta");
	//transform.localScale=Vector3(scale, 1 ,scale);
}
function onRequestAlphaa(a:float){
	scale=a;
	}