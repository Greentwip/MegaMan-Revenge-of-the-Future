using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    #region Variables

    // Properties
    public float HealthStatus { get; set; }

    // Public Instance Variables
    public Texture2D EmptyTex = null;
    public Texture2D FullTex = null;

    public bool ShowHealthBar = true;
    public Vector2 Position = Vector2.zero;


    // Protected Instance Variables
    protected Vector2 size = Vector2.one;

    // Private Instance Variables
    private Health health = null;

    #endregion


    #region MonoBehaviour


    private void Start()
    {
        health = this.GetComponent<Health>();
    }

    protected void OnGUI()
    {
        if (ShowHealthBar == true)
        {
            HealthStatus = health.currentHealth / health.MaximumHealth;

            size = new Vector2(Screen.width / 42f, Screen.height / 6f);

            //draw the background:
            GUI.BeginGroup(new Rect(Position.x, Position.y, size.x, size.y));
            {
                GUIStyle gg = new GUIStyle();

                GUI.Box(new Rect(0, 0, size.x, size.y), FullTex, gg);

                //draw the filled-in part:
                GUI.BeginGroup(new Rect(0, 0, size.x, size.y - size.y * HealthStatus));
                {
                    GUI.Box(new Rect(0, 0, size.x, size.y), EmptyTex, gg);
                }
                GUI.EndGroup();
            }
            GUI.EndGroup();
        }
    }

    #endregion
}
