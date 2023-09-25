using UnityEngine;

public class AnimatorComponent : Component
{
    [SerializeField] public Animator animator;

    public void Initialize(Animator animator)
    {
        this.animator = animator;
    }
    public bool GetBool(string name)
    {
        return animator.GetBool(name);
    }

    public void SetBool(string name, bool value)
    {
        animator.SetBool(name, value);
    }

    public void SetFloat(string name, float value)
    {
        animator.SetFloat(name, value);
    }

    public void SetInt(string name, int value)
    {
        animator.SetInteger(name, value);
    }
}
