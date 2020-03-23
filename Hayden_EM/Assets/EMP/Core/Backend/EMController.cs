using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EMP.Core
{
    public class EMController : Singleton<EMController>
    {
        // Keep a list of references to all FieldGenerators in the scene.
        private List<FieldGenerator> fieldGenerators = new List<FieldGenerator>();

        // Keep a list of references to all FieldReactors in the scene.
        private List<FieldReactor> fieldReactors = new List<FieldReactor>();
        
        // Keep a list of all InteractionFields that are or have been present in the scene.
        private List<InteractionField> fieldsInScene = new List<InteractionField>();
        
        public delegate void FieldGeneratorRegistrationHandler (FieldGenerator generator);

        public delegate void FieldReactorRegistrationHandler(FieldReactor reactor);

        public delegate void InteractionFieldRegistrationHandler(InteractionField field);
 
        // Field generator registration events
        public event FieldGeneratorRegistrationHandler OnFieldGeneratorRegistered;
        public event FieldGeneratorRegistrationHandler OnFieldGeneratorUnregistered;
        
        // Field reactor registration events
        public event FieldReactorRegistrationHandler OnFieldReactorRegistered;
        public event FieldReactorRegistrationHandler OnFieldReactorUnregistered;
        
        // Interaction field registration event
        public event InteractionFieldRegistrationHandler OnInteractionFieldRegistered;
        
        // Each physics update tick, each FieldReactor calculates and applies relevant forces and torques upon itself.
        private void FixedUpdate()
        {
            foreach (FieldReactor FR in fieldReactors)
            {
                FR.ApplyForces();
                FR.ApplyTorques();
            }
        }
        
        /// <summary>
        /// Register a FieldGenerator with EMController. Called automatically by every FieldGenerator on scene start or
        /// upon its creation, whichever is earlier.
        /// </summary>
        /// <param name="NewFieldGenerator">FieldGenerator to register.</param>
        public void RegisterFieldGenerator(FieldGenerator NewFieldGenerator)
        {
            // Make sure new field generator isn't already registered
            if (fieldGenerators.Contains(NewFieldGenerator))
            {
                return;
            }
            // Add new field generator
            fieldGenerators.Add(NewFieldGenerator);
            
            // Register field type
            RegisterFieldType(NewFieldGenerator.generatedField);
            OnFieldGeneratorRegistered?.Invoke(NewFieldGenerator);
        }
        
        /// <summary>
        /// Removes a FieldGenerator from EMController's registry. Called automatically by all FieldGenerators on their
        /// destruction.
        /// </summary>
        /// <param name="RemoveFieldGenerator">FieldGenerator to remove.</param>
        public void UnRegisterFieldGenerator(FieldGenerator RemoveFieldGenerator)
        {
            fieldGenerators.Remove(RemoveFieldGenerator);
            OnFieldGeneratorUnregistered?.Invoke(RemoveFieldGenerator);
        }

        /// <summary>
        /// Register a FieldReactor with EMController. Called automatically by every FieldReactor on scene start or
        /// upon its creation, whichever is earlier.
        /// </summary>
        /// <param name="NewFieldReactor">FieldReactor to register.</param>
        public void RegisterFieldReactor(FieldReactor NewFieldReactor)
        {
            // Make sure new field reactor isn't already registered
            if (fieldReactors.Contains(NewFieldReactor))
            {
                return;
            }
            // Add new field reactor
            fieldReactors.Add(NewFieldReactor);
            
            // Register field type
            RegisterFieldType(NewFieldReactor.fieldReactingTo);
            OnFieldReactorRegistered?.Invoke(NewFieldReactor);
        }
        
        /// <summary>
        /// Removes a FieldReactor from EMController's registry. Called automatically by all FieldReactors on their
        /// destruction.
        /// </summary>
        /// <param name="RemoveFieldReactor">FieldReactor to remove.</param>
        public void UnRegisterFieldReactor(FieldReactor RemoveFieldReactor)
        {
            fieldReactors.Remove(RemoveFieldReactor);
            OnFieldReactorUnregistered?.Invoke(RemoveFieldReactor);
        }
        
        /// <summary>
        /// Registers a new InteractionField as being present in the scene.
        /// </summary>
        /// <param name="field"></param>
        private void RegisterFieldType(InteractionField field)
        {
            if (!fieldsInScene.Contains(field))
            {
                fieldsInScene.Add(field);
                OnInteractionFieldRegistered?.Invoke(field);
            }
        }
    
        
        /// <summary>
        /// Calculate the value of the <paramref name="field"/> at world-space <paramref name="position"/>.
        /// If <paramref name="generatorToExclude"/> is specified, it is left out of the calculation, e.g. for
        /// calculating the electric field at a point charge due to other charges.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="field"></param>
        /// <param name="generatorToExclude"></param>
        /// <returns>Direction and magnitude of the field.</returns>
        public Vector3 GetFieldValue(Vector3 position, InteractionField field, FieldGenerator generatorToExclude = null)
        {
            Vector3 fieldValue = new Vector3(0, 0, 0);

            foreach (FieldGenerator fieldGenerator in fieldGenerators)
            {
                if (generatorToExclude)
                {
                    if (fieldGenerator == generatorToExclude)
                    {
                        continue;
                    }
                }
                if (fieldGenerator.generatedField == field)
                {
                    fieldValue = fieldValue + fieldGenerator.FieldValue(position);
                }
            }

            return fieldValue;
        }
        
        /// <summary>
        /// Gets a list of all FieldGenerators of type <typeparamref name="T"/> which generate the specified
        /// <paramref name="field"/>.
        /// </summary>
        /// <param name="field"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetFieldGenerators<T>(InteractionField field) where T: FieldGenerator
        {
            List<T> outList = new List<T>();
            foreach (FieldGenerator generator in fieldGenerators)
            {
                if (generator.GetType() == typeof(T) && field == generator.generatedField)
                {
                    outList.Add((T)generator);
                }
            }

            return outList;
        }
    }
}