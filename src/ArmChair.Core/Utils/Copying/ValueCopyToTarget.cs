namespace ArmChair.Utils.Copying
{
    public class ValueCopyToTarget : CopyToTarget
    {
        private readonly FieldMeta _field;

        public ValueCopyToTarget(FieldMeta field)
        {
            _field = field;
        }

        public override void Copy(object source, object destination)
        {
            var value = _field.GetFieldValueFor(source);
            _field.SetFieldValueOf(destination, value);
        }
    }
}