public static class Constants
{
    public const float MinimalMoveInputSecu = 0.25f;
    public const float MinimalRotaInputSecu = 0.09f;
    public const float MaxStepHeight = 0.45f;
    public const float MinPlayerRotationAngle = 0.05f;
    /// <summary>
    /// <para>Not used as the real physic parameter </para>
    /// <para>Only to differenciate graph representations of different overlapping RayCasts</para>
    /// </summary>
    public const float VirtualGraphRayCastOriginOffset = 0.1f;

    /// <summary>
    /// <para>In case Unity Objects in scene are not perfectly vertical</para>
    /// </summary>
    public const float MinDotSecuVal = 0.001f;

    public const float MaxSlipContactAngle = 90f;
    public const float RespawnOffsetHeight = 0.5f;
}