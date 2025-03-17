using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimate : CharacterAppearance
{
    // Current animation state
    [SerializeField] private CharacterAnimation currentAnimation = CharacterAnimation.StandFront;
    
    // Animation frame data - maps animation states to sequences of sprite indices
    private Dictionary<CharacterAnimation, int[]> animations = new Dictionary<CharacterAnimation, int[]>()
    {
        { CharacterAnimation.WalkLeft, new int[] { 12, 13, 14, 15 } },
        { CharacterAnimation.WalkRight, new int[] { 4, 5, 6, 7 } },
        { CharacterAnimation.WalkFront, new int[] { 8, 9, 10, 11 } },
        { CharacterAnimation.WalkBack, new int[] { 0, 1, 2, 3 } },
        { CharacterAnimation.StandBack, new int[] { 1 } },
        { CharacterAnimation.StandRight, new int[] { 5 } },
        { CharacterAnimation.StandLeft, new int[] { 13 } },
        { CharacterAnimation.StandFront, new int[] { 9 } }
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
        if (Input.GetKeyDown(KeyCode.V) && Main.instance.vChangesCharacterAnimation)
        {
            // random animation
            CharacterAnimation[] animations = (CharacterAnimation[])System.Enum.GetValues(typeof(CharacterAnimation));
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
    public void SetAnimation(CharacterAnimation animation)
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
    public CharacterAnimation GetCurrentAnimation()
    {
        return currentAnimation;
    }
    
    // Enable or disable animation
    public void SetAnimationEnabled(bool enabled)
    {
        animationEnabled = enabled;
    }
    
    // Helper methods for common state changes
    public void StandInDirection(Vector2 direction)
    {
        // Convert direction to the closest cardinal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement dominant
            if (direction.x > 0)
                SetAnimation(CharacterAnimation.StandRight);
            else
                SetAnimation(CharacterAnimation.StandLeft);
        }
        else
        {
            // Vertical movement dominant
            if (direction.y > 0)
                SetAnimation(CharacterAnimation.StandBack);
            else
                SetAnimation(CharacterAnimation.StandFront);
        }
    }
    
    public void WalkInDirection(Vector2 direction)
    {
        // Convert direction to the closest cardinal direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Horizontal movement dominant
            if (direction.x > 0)
                SetAnimation(CharacterAnimation.WalkRight);
            else
                SetAnimation(CharacterAnimation.WalkLeft);
        }
        else
        {
            // Vertical movement dominant
            if (direction.y > 0)
                SetAnimation(CharacterAnimation.WalkBack);
            else
                SetAnimation(CharacterAnimation.WalkFront);
        }
    }
}