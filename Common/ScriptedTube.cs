// TODO: Maybe add an exit direction to this?
using UnityEngine;

namespace FP2_Knux_Objects.Common
{
    public class ScriptedTube : FPBaseObject
    {
        // FPBaseObject stuff.
        private static int classID = -1;
        private FPObjectState state;
        private bool isValidatedInObjectList;
        private FPHitBox hbTouch;

        // The points that make up this tube's path.
        public Vector2[] points = [];

        // An index to track the current point we're targeting.
        private int pointIndex;

        private new void Start()
        {
            // Check if the points have been set. If not, then report it and kill this monitor.
            if (points.Length == 0)
            {
                Plugin.consoleLog.LogError($"Scripted Tube without any points found at {transform.position}! Removing.");
                GameObject.Destroy(gameObject);
                return;
            }

            state = State_Default;

            // Set up the tube's hitbox.
            hbTouch.left = -32;
            hbTouch.top = 32;
            hbTouch.right = 32;
            hbTouch.bottom = -32;
            hbTouch.enabled = true;
            hbTouch.visible = true;

            // Start the FPBaseObject setup.
            base.Start();
            classID = FPStage.RegisterObjectType(this, GetType(), 0);
            objectID = classID;
        }

        private void Update()
        {
            // Validate this object in the stage list if it hasn't already been.
            if (!isValidatedInObjectList && FPStage.objectsRegistered)
                isValidatedInObjectList = FPStage.ValidateStageListPos(this);

            // Invoke the current state if it isn't null.
            state?.Invoke();
        }

        private void State_Default()
        {
            // Set up a value to hold an object.
            FPBaseObject objRef = null;

            // Loop through each player object in the stage.
            while (FPStage.ForEach(FPPlayer.classID, ref objRef))
            {
                // Get a reference to this player.
                FPPlayer fPPlayer = (FPPlayer)objRef;

                // Check if the player's collider has overlapped this tube's.
                if (FPCollision.CheckOOBB(this, hbTouch, objRef, fPPlayer.hbTouch))
                {
                    // Set the player to our custom scripted tube state.
                    fPPlayer.state = Patchers.FPPlayerPatcher.State_Scripted_Tube;

                    // Set this tube to the transporting state.
                    state = State_Transporting;

                    // Reset the point index.
                    pointIndex = 0;

                    // Set the player onto the Foreground C plane so they don't collide with the terrain.
                    objRef.collisionLayer = 0x400;

                    // Check that the player isn't already in the rolling animation.
                    if (fPPlayer.currentAnimation != "Rolling")
                    {
                        // Set the player's animation to the rolling one.
                        fPPlayer.SetPlayerAnimation("Rolling");

                        // Play the roll sound.
                        fPPlayer.Action_PlaySoundUninterruptable(fPPlayer.sfxRolling);
                    }
                }
            }
        }

        private void State_Transporting()
        {
            // Set up a value to hold an object.
            FPBaseObject objRef = null;

            // Loop through each player object in the stage.
            while (FPStage.ForEach(FPPlayer.classID, ref objRef))
            {
                // Get a reference to this player.
                FPPlayer fPPlayer = (FPPlayer)objRef;

                // Check if we've reached the last point.
                if (pointIndex >= points.Length)
                {
                    // Return this tube to its default state.
                    state = State_Default;

                    // Change the player to the physics ball state.
                    fPPlayer.state = fPPlayer.State_Ball_Physics;

                    // Forcibly give the player a push to the right.
                    fPPlayer.velocity.x = 32f;

                    // Reset the player back to the Foreground A plane.
                    objRef.collisionLayer = 0x100;

                    // Don't bother with the rest of this function.
                    return;
                }

                // Check if the player isn't at the same position as the current point.
                // If they aren't, then move them towards it, if they are, the increment the index.
                if (fPPlayer.position != points[pointIndex])
                    fPPlayer.position = Vector2.MoveTowards(fPPlayer.position, points[pointIndex], 32f);
                else
                    pointIndex++;
            }
        }
    }
}
