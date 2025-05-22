using System.Linq;

namespace FP2_Knux_Objects.Common
{
    public class ZoomTubePoint : FPBaseObject
    {
        // FPBaseObject stuff.
        private static int classID = -1;
        private FPObjectState state;
        private bool isValidatedInObjectList;

        // The hitboxes for this tube.
        private FPHitBox hbEnter;
        private FPHitBox hbExit;

        // The direction that the exit hitbox is shifted by.
        public ZoomTubeExitDirection exitDirection;

        // Animations that this Zoom Tube Point should not activate on.
        public string[] BlacklistedAnimations = [];

        private new void Start()
        {
            state = State_Default;

            // Set up the tube's hitbox.
            hbEnter.left = -32;
            hbEnter.top = 32;
            hbEnter.right = 32;
            hbEnter.bottom = -32;
            hbEnter.enabled = true;
            hbEnter.visible = true;

            // Set up the tube's exit hitbox depending on the direction.
            switch (exitDirection)
            {
                case ZoomTubeExitDirection.Up:
                    hbExit.left = -32;
                    hbExit.top = 96;
                    hbExit.right = 32;
                    hbExit.bottom = 32;
                    break;
                case ZoomTubeExitDirection.Down:
                    hbExit.left = -32;
                    hbExit.top = -32;
                    hbExit.right = 32;
                    hbExit.bottom = -96;
                    break;
                case ZoomTubeExitDirection.Left:
                    hbExit.left = -96;
                    hbExit.top = 32;
                    hbExit.right = -32;
                    hbExit.bottom = -32;
                    break;
                case ZoomTubeExitDirection.Right:
                    hbExit.left = 32;
                    hbExit.top = 32;
                    hbExit.right = 96;
                    hbExit.bottom = -32;
                    break;
            }
            hbExit.enabled = true;
            hbExit.visible = true;

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

                // Check if the player's collider has overlapped this tube's entrance hitbox.
                if (FPCollision.CheckOOBB(this, hbEnter, objRef, fPPlayer.hbTouch))
                {
                    // Check if the player isn't already in the physics ball state or in a blacklisted animation.
                    if (fPPlayer.state != fPPlayer.State_Ball_Physics && !BlacklistedAnimations.Contains(fPPlayer.currentAnimation))
                    {
                        // Set the player to the physics ball state.
                        fPPlayer.state = fPPlayer.State_Ball_Physics;

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

                // Check if the player's collider has overlapped this tube's exit hitbox.
                if (FPCollision.CheckOOBB(this, hbExit, objRef, fPPlayer.hbTouch))
                {
                    // Check that the player is in the physics ball state and set them to Carol's roll state if so.
                    if (fPPlayer.state == fPPlayer.State_Ball_Physics)
                        fPPlayer.state = fPPlayer.State_Carol_Roll;
                }
            }
        }
    }
}
