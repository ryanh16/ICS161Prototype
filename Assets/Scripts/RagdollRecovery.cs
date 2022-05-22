using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollRecovery : MonoBehaviour 
{
	[Header("The name of the state that should be checked after recovery animation")]
	public string recoveredStateName;
    //Possible states of the ragdoll
	public enum RagdollState
	{
		standing,	    
		ragdolled,     
		blending       //blend state (from ragdoll to standing up)
	}
	
	//The current state
	internal RagdollState state = RagdollState.standing;
	
	//How long do we blend when transitioning from ragdolled to standing
	public float standingBlendTime = 0.5f;
	public float standingTransitionTime = 0.1f;
	
	//A helper variable to store the time when we transitioned from ragdolled to blendToAnim state
	float ragdollEndTime;
	
	//Declare a class that will hold useful information for each body part
	public class Limb
	{
		public Transform transform;
		public Vector3 position;
		public Quaternion rotation;

	}
	
	Transform rootTransform;

	Vector3 hipPosition;
	Vector3 headPosition;
	Vector3 feetPosition;
	
	//Declare a list of body parts, initialized in Start()
	List<Limb> ragdollLimbs = new List<Limb>();
	
	//Declare an Animator member variable, initialized in Start to point to this gameobject's Animator component.
	Animator animator;

	RagdollToggle ragdollToggle;
	
	// Initialization, first frame of game
	void Start ()
	{
		animator = GetComponent<Animator>();

		rootTransform = transform.Find("Root");
		//For each of the transforms, create a BodyPart instance and store the transform 
		foreach (Transform child in rootTransform)
		{
			Limb limb = new Limb();
			limb.transform = child;
			ragdollLimbs.Add(limb);
		}

		ragdollToggle = GetComponent<RagdollToggle>();
		
	}

    float calculateBlendTime()
    {
        float time = 1 - (Time.time - ragdollEndTime - standingTransitionTime) / standingBlendTime;
		// Debug.Log($"Time: {time} Clamped time: {Mathf.Clamp01(time)}");
        return Mathf.Clamp01(time);
    }
	void calculateRagdollOrientation()
	{
		//update position where the ragdoll is and attempt to blend anims together
		Vector3 updatedHipPosition = hipPosition
										- animator.GetBoneTransform(HumanBodyBones.Hips).position;
		Vector3 updatedRootPosition = rootTransform.position //transform.position    
										+ updatedHipPosition;
		// Debug.Log($"updated hip position: {updatedHipPosition}, updated root position: {updatedRootPosition}");	
		updatedRootPosition.y = 0;

		//find ground
		RaycastHit[] groundCheck = Physics.RaycastAll(new Ray(updatedRootPosition,Vector3.down)); 
		
		foreach(RaycastHit hit in groundCheck)
		{
			//pick highest y position to use for the root's position
			if (!hit.transform.IsChildOf(transform))
			{
				updatedRootPosition.y=Mathf.Max(updatedRootPosition.y, hit.point.y);
			}
		}
		transform.position = updatedRootPosition;
		
		//find orientation of ragdoll
		Vector3 ragdollOrientation = headPosition - feetPosition;
		ragdollOrientation.y = 0;

		Vector3 meanFeetPosition = 0.5f * (animator.GetBoneTransform(HumanBodyBones.LeftFoot).position 
									+ animator.GetBoneTransform(HumanBodyBones.RightFoot).position);
		Vector3 standingDirection = animator.GetBoneTransform(HumanBodyBones.Head).position 
									- meanFeetPosition;
		standingDirection.y = 0;
								
		//try to get rotation of body
		transform.rotation *= Quaternion.FromToRotation(standingDirection.normalized,ragdollOrientation.normalized);
	}

	void LateUpdate()
	{
		// Debug.Log($"Hip forward: {animator.GetBoneTransform(HumanBodyBones.Hips).up}");
		// animator.SetBool("StandUpFromBack",false);
		// animator.SetBool("StandUpFromFront",false);

		switch(state)
		{
			case RagdollState.ragdolled:
					ragdollEndTime = Time.time; //store time state changed

					// animator.enabled = true; //enable animation
					state = RagdollState.blending;
					
					//Store the ragdoll limb positions
					foreach (Limb limb in ragdollLimbs)
					{
						limb.position = limb.transform.position;
						limb.rotation = limb.transform.rotation;
					}
					
					//store positions
					feetPosition = 0.5f * 
									(animator.GetBoneTransform(HumanBodyBones.LeftToes).position 
									+ animator.GetBoneTransform(HumanBodyBones.RightToes).position);
					headPosition = animator.GetBoneTransform(HumanBodyBones.Head).position;
					hipPosition = animator.GetBoneTransform(HumanBodyBones.Hips).position;
					
					animator.enabled = true;

					//select random option for animations for each stand up animation
					int animChoice = Random.Range(0,2);

					// Debug.Log($"Hip forward: {animator.GetBoneTransform(HumanBodyBones.Hips).up}");
					//if hips facing upward, stand up from the back, else stand up from the front
					if (animator.GetBoneTransform(HumanBodyBones.Hips).up.y > 0)
					{
						animator.SetFloat("StandOption", animChoice);
						animator.SetBool("StandUpFromBack",true);
					}
					else
					{
						animator.SetFloat("StandOption", animChoice);
						animator.SetBool("StandUpFromFront",true);
					}
				break;
			case RagdollState.blending:

					if (Time.time <= ragdollEndTime + standingTransitionTime)
					{
						//clear bools to stop animations
						animator.SetBool("StandUpFromBack", false);
						animator.SetBool("StandUpFromFront", false);
						calculateRagdollOrientation();
					}

					float blend_t_value = calculateBlendTime();
					
					//lerp and slerp between ragdoll and animation
					foreach (Limb limb in ragdollLimbs)
					{
						//if not root transform
						if (limb.transform != rootTransform)
						{ 
							
							//lerp hip position, slerp limb rotations
							if (limb.transform == animator.GetBoneTransform(HumanBodyBones.Hips))
							{
								limb.transform.position = Vector3.Lerp(limb.transform.position, limb.position, blend_t_value);
							}
							limb.transform.rotation = Quaternion.Slerp(limb.transform.rotation, limb.rotation, blend_t_value);
						}
					}
					
					//once blend value is done and state is back to original animation, set state to standing
					if (blend_t_value == 0 && isFullyRecovered())//&& animator.GetCurrentAnimatorStateInfo(0).IsName("Locomotion"))
					{
						// transform.position = new Vector3(transform.position.x, originalHeight, transform.position.z);
						// transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, 0, transform.rotation.z));
						state = RagdollState.standing;
						ragdollToggle.disableRagdoll();
						break;
					}
				break;
			case RagdollState.standing:
					// originalHeight = transform.position.y;
				break;
		}
	}
	
	bool isFullyRecovered()
	{
		//checks if fully recovered by checking if the name matches with the anim state
		Debug.Log($"name of anim after recovery: {recoveredStateName}, status = {animator.GetCurrentAnimatorStateInfo(0).IsName(recoveredStateName)} ");
		return animator.GetCurrentAnimatorStateInfo(0).IsName(recoveredStateName);
	}

}
