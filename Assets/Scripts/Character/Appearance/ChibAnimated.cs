using System.Collections.Generic;
using UnityEngine;

public enum Pose
{
    StandFront, StandBack, StandLeft, StandRight,
    WalkFront, WalkBack, WalkLeft, WalkRight,
    FallFront, FallBack, FallLeft, FallRight
}

public interface IAnimated
{
    void SetAnimation(Pose animation);
    Pose GetCurrentAnimation();
    void SetAnimationEnabled(bool enabled);
    void StandInDirection(Vector2 direction);
    void FallInDirection(Vector2 direction);
    void WalkInDirection(Vector2 direction);
}

public class ChibAnimated : ChibSprite, IAnimated
{
    // Current animation state
    [SerializeField] private Pose currentAnimation = Pose.StandFront;
    
    // Animation frame data - maps animation states to sequences of sprite indices
    private Dictionary<Pose, int[]> animations = new Dictionary<Pose, int[]>()
    {
        { Pose.WalkLeft, new int[] { 12, 13, 14, 15 } },
        { Pose.WalkRight, new int[] { 4, 5, 6, 7 } },
        { Pose.WalkFront, new int[] { 8, 9, 10, 11 } },
        { Pose.WalkBack, new int[] { 0, 1, 2, 3 } },
        { Pose.StandBack, new int[] { 1 } },
        { Pose.StandRight, new int[] { 5 } },
        { Pose.StandLeft, new int[] { 13 } },
        { Pose.StandFront, new int[] { 9 } },
        { Pose.FallBack, new int[] { 2 } },
        { Pose.FallRight, new int[] { 6 } },
        { Pose.FallLeft, new int[] { 14 } },
        { Pose.FallFront, new int[] { 10 } }
    };
    
    // Animation properties
    [SerializeField] private float frameRate; // 6
    private float frameTimer = 0f;
    private int currentFrame = 0;
    
    // Animation enabled flag
    [SerializeField] private bool animationEnabled = true;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start() 
    {
        base.Start();
    }
    
    protected override void Update()
    {
        // Handle V key for debugging
        if (Input.GetKeyDown(KeyCode.V) && DevTools.instance.vChangesCharacterAnimation)
        {
            // random animation
            Pose[] animations = (Pose[])System.Enum.GetValues(typeof(Pose));
            SetAnimation(animations[Random.Range(0, animations.Length)]);
        }
        
        // Handle animation if enabled
        if (animationEnabled)
        {
            UpdateAnimation();
        }

        base.Update();
    }
    
    // Update the animation frame based on time
    private void UpdateAnimation()
    {
        // Only animate if we have frames for the current animation
        if (animations.TryGetValue(currentAnimation, out int[] frames) && frames.Length > 1)
        {
            // Update timer
            frameTimer += Time.deltaTime;
            
            // Time to change to the next frame?
            if (frameTimer >= 1f / frameRate)
            {
                // Reset timer and advance to next frame
                frameTimer = 0f;
                currentFrame = (currentFrame + 1) % frames.Length;
                
                // Apply the new sprite index
                SetSpriteIndex(frames[currentFrame]);
            }
        }
    }
    
    // Change the current animation state
    public void SetAnimation(Pose animation)
    {
        // Only change if it's different
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            currentFrame = 0; // Reset to first frame
            frameTimer = 0f;  // Reset timer
            
            // Apply first frame of new animation
            if (animations.TryGetValue(animation, out int[] frames) && frames.Length > 0)
            {
                SetSpriteIndex(frames[0]);
            }
        }
    }
    
    // Get the current animation state
    public Pose GetCurrentAnimation()
    {
        return currentAnimation;
    }
    
    // Enable or disable animation
    public void SetAnimationEnabled(bool enabled)
    {
        animationEnabled = enabled;
    }
    
    public void StandInDirection(Vector2 direction)
    {
        // Determine closest cardinal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0) SetAnimation(Pose.StandRight);
            else SetAnimation(Pose.StandLeft);
        }
        else
        {
            if (direction.y > 0) SetAnimation(Pose.StandBack);
            else SetAnimation(Pose.StandFront);
        }
    }

    public void FallInDirection(Vector2 direction)
    {
        // Determine closest cardinal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) 
        {
            if (direction.x > 0) SetAnimation(Pose.FallRight);
            else SetAnimation(Pose.FallLeft);
        }
        else
        {
            if (direction.y > 0) SetAnimation(Pose.FallBack);
            else SetAnimation(Pose.FallFront);
        }
    }
    
    public void WalkInDirection(Vector2 direction)
    {
        // Convert direction to the closest cardinal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement dominant
            if (direction.x > 0)
                SetAnimation(Pose.WalkRight);
            else
                SetAnimation(Pose.WalkLeft);
        }
        else
        {
            // Vertical movement dominant
            if (direction.y > 0)
                SetAnimation(Pose.WalkBack);
            else
                SetAnimation(Pose.WalkFront);
        }
    }
}