namespace ArmChair.Utils.Copying
{
    public interface ICopyToTarget
    {
        void Copy(object source, object destination);
        void Congfigure(ShadowCopier copier);
    }
}