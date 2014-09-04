namespace ArmChair.Utils.Copying
{
    public abstract class CopyToTarget : ICopyToTarget
    {
        protected ShadowCopier Copier;

        public abstract void Copy(object source, object destination);
        public virtual void Congfigure(ShadowCopier copier)
        {
            Copier = copier;
        }
    }
}