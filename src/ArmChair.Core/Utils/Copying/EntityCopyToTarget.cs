using System;

namespace ArmChair.Utils.Copying
{
    public class EntityCopyToTarget : CopyToTarget
    {
        private readonly FieldMeta _field;
        private readonly Action<object, object> _action;

        public EntityCopyToTarget(FieldMeta field)
        {
            _field = field;

            _action = _field.IsReadOnly
                ? (Action<object, object>)((s, d) =>
                {
                    var dest = field.GetFieldValueFor(d);
                    Copier.Copy(s, dest);
                })
                : (Action<object, object>)((s, d) =>
                {
                    var copy = Copier.Copy(s);
                    _field.SetFieldValueOf(d, copy);
                });
        }

        public override void Copy(object source, object destination)
        {
            var value = _field.GetFieldValueFor(source);
            _action(value, destination);

        }
    }
}