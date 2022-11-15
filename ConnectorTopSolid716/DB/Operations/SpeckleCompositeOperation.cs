using TopSolid.Kernel.DB.Parameters;
using System.Runtime.InteropServices;
using TopSolid.Cad.Design.DB;
using TopSolid.Cad.Design.DB.Parts;
using TopSolid.Kernel.DB.D3.Directions;
using TopSolid.Kernel.DB.D3.Planes;
using TopSolid.Kernel.DB.D3.Sections;
using TopSolid.Kernel.DB.D3.Shapes;
using TopSolid.Kernel.DB.D3.Shapes.Bosses;
using TopSolid.Kernel.DB.D3.Shapes.Pockets;
using TopSolid.Kernel.DB.Documents;
using TopSolid.Kernel.DB.Elements;
using TopSolid.Kernel.DB.Operations;
using TopSolid.Kernel.DB.SmartObjects;
using TopSolid.Kernel.G.D3;
using TopSolid.Kernel.G.D3.Shapes.Extruded;
using TopSolid.Kernel.TX.Units;
using TopSolid.Kernel.SX.Collections.Generic;
using Extent = TopSolid.Kernel.G.D2.Extent;

namespace Speckle.ConnectorTopSolid.DB.Operations
{
    [Guid("86E85324-254E-4DB1-A9A8-9456FC6430B3")]
    public sealed partial class SpeckleCompositeOperation : CreationsCompositeOperation
    {
       

        /// <summary>
        /// The pocket operation
        /// </summary>
        private PocketOperation pocketOperation;

        // Constructors:

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeckleCompositeOperation"/> class.
        /// </summary>
        /// <param name="inDocument">Container document (referenced).</param>
        /// <param name="inId">Element identifier, or zero for automatic.</param>
        public SpeckleCompositeOperation(TopSolid.Kernel.DB.Documents.Document inDocument, int inId)
            : base(inDocument, inId)
        {
         
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeckleCompositeOperation"/> class by reading
        /// data from a stream.
        /// </summary>
        /// <param name="inReader">Reader to use.</param>
        /// <param name="inDocument">Container document (referenced).</param>
        private SpeckleCompositeOperation(TopSolid.Kernel.SX.IO.IReader inReader, object inDocument)
            : base(inReader, inDocument)
        {
           

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeckleCompositeOperation"/> class by copy.
        /// </summary>
        /// <param name="inOriginal">Original instance to copy.</param>
        /// <param name="inDocument">Container document (referenced).</param>
        /// <param name="inId">Clone element identifier, or zero for automatic.</param>
        private SpeckleCompositeOperation(SpeckleCompositeOperation inOriginal, TopSolid.Kernel.DB.Documents.Document inDocument, int inId)
            : base(inOriginal, inDocument, inId)
        {
           
        }

        /// <summary>
        /// Clones this element.
        /// </summary>
        /// <param name="inDocument">Document that will contain the element clone.</param>
        /// <param name="inId">Clone element identifier, or zero for automatic.</param>
        /// <returns>
        /// New clone of this element.
        /// </returns>
        public override TopSolid.Kernel.DB.Elements.Element Clone(TopSolid.Kernel.DB.Documents.Document inDocument, int inId)
        {
            return new SpeckleCompositeOperation(this, inDocument, inId);
        }

       

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public SmartReal Offset
        {
            get
            {
                return this.offsetHandle.SmartObject;
            }
            set
            {
                this.offsetHandle.SmartObject = value;
            }
        }

        /// <summary>
        /// Gets or sets the boss direction.
        /// </summary>
        public SmartDirection Direction
        {
            get
            {
                return this.directionHandle.SmartObject;
            }
            set
            {
                this.directionHandle.SmartObject = value;
            }
        }

        /// <inheritdoc/>
        public override bool RequiresName
        {
            get
            {
                return false;
            }
        }

     

        // Methods:

        /// <summary>
        /// </summary>
        /// <param name="outRefs"></param>
        /// <inheritdoc />
        public override void GetReferences(TopSolid.Kernel.DB.References.ReferenceList outRefs)
        {
            base.GetReferences(outRefs);
            if (this.femalePartHandle != null)
            {
                this.femalePartHandle.GetReferences(outRefs);
            }
            if (this.malePartHandle != null)
            {
                this.malePartHandle.GetReferences(outRefs);
            }
            this.femaleEntityOperationPair.GetReferences(outRefs);
            this.maleEntityOperationPair.GetReferences(outRefs);
            this.sectionHandle.GetReferences(outRefs);
            this.directionHandle.GetReferences(outRefs);
            this.offsetHandle.GetReferences(outRefs);
        }

        public override void GetChildren(bool inIgnoresOperations, ElementList outChildren)
        {
            base.GetChildren(inIgnoresOperations, outChildren);
            this.femaleEntityOperationPair.GetChildren(inIgnoresOperations, outChildren);
            this.maleEntityOperationPair.GetChildren(inIgnoresOperations, outChildren);
        }

        /// <summary>
        /// </summary>
        /// <param name="outHandles"></param>
        /// <inheritdoc />
        public override void GetSmartObjectHandles(SmartObjectHandleList outHandles)
        {
            base.GetSmartObjectHandles(outHandles);
            outHandles.Add(this.femalePartHandle);
            outHandles.Add(this.malePartHandle);
            outHandles.Add(this.sectionHandle);
            outHandles.Add(this.directionHandle);
            outHandles.Add(this.offsetHandle);
        }

        /// <summary>
        /// Executes the operation.
        /// </summary>
        /// <remarks>
        /// <para>Every operation must implement this method.</para>
        /// <para>When the execution cannot be performed, the method should set the <see cref="P:TopSolid.Kernel.DB.Operations.Operation.IsInvalid" /> property
        /// to <c>true</c> before returning.</para>
        /// <para>When the operation needs to be deleted instead of being executed, the method should set the <see cref="P:TopSolid.Kernel.DB.Operations.Operation.NeedsDeleting" />
        /// property to <c>true</c> before returning.</para>
        /// </remarks>
        protected override void Execute()
        {
            //Check if node occurrences have targets
            if ((this.femalePartHandle == null && this.malePartHandle == null && this.femaleEntityOperationPair == null && this.maleEntityOperationPair == null)
               || (this.femalePartHandle != null && this.femalePartHandle.IsEmpty) && (this.malePartHandle != null && this.malePartHandle.IsEmpty))
            {
                this.FemalePart = null;
                this.MalePart = null;
                this.NeedsDeleting = true;
                return;
            }

            // Check parts.
            if (this.femalePartHandle != null && this.femalePartHandle.IsInvalid && this.malePartHandle != null && this.malePartHandle.IsInvalid)
            {
                if (this.FemalePart != null && this.MalePart != null)
                {
                    if (this.Document.IsUpdateHealing)
                    {
                        this.IsInvalid = true;
                        return;
                    }
                    else
                    {
                        this.IsInvalid = true;
                        return;
                    }
                }

                this.IsInvalid = true;
                return;
            }

            // Update part entity and operation pairs.
            this.UpdatePartEntityOperationPairs();

            #region Pocket Operation
            //Get the operation
            pocketOperation = femaleEntityOperationPair.Operation as PocketOperation;

            //Create
            if (pocketOperation == null)
            {
                pocketOperation = new PocketOperation(femaleEntityOperationPair.PartEntity.GetDocumentHostingModificationOperations(), 0);
                pocketOperation.IsDeletable = false;
                pocketOperation.IsModifiable = false;
                pocketOperation.IsMovable = false;
            }
            //Modify
            else
            {
                Document opDocument = pocketOperation.Document;
                Document shEntitiesDocument = femaleEntityOperationPair.PartEntity.GetDocumentHostingModificationOperations();

                if (opDocument != shEntitiesDocument)
                {
                    Operation.Delete(pocketOperation);
                    pocketOperation = new PocketOperation(femaleEntityOperationPair.PartEntity.GetDocumentHostingModificationOperations(), 0);
                    pocketOperation.IsDeletable = false;
                    pocketOperation.IsModifiable = false;
                    pocketOperation.IsMovable = false;
                }
            }

            //Get female part entity transform
            Transform pocketTransform = femaleEntityOperationPair.PartEntity.OccurrenceDefinitionTransform;

            //Get the shape to modify
            List<ShapeEntity> shapeEntities = new List<ShapeEntity>();
            femaleEntityOperationPair.PartEntity.GetShapesToModify(false, true, shapeEntities);
            pocketOperation.ModifiedEntity = shapeEntities[0];

            //Transform and set the section
            TopSolid.Kernel.G.D3.Curves.IGeometricSection section = this.Section.Geometry.Clone() as TopSolid.Kernel.G.D3.Curves.IGeometricSection;
            section.TransformByInverse(pocketTransform);
            pocketOperation.Section = new BasicSmartSection(pocketOperation, section);

            //Transform and set the direction
            UnitVector pocketDirection = this.Direction.Geometry;
            pocketDirection.TransformByInverse(pocketTransform);
            Point pocketHelpPoint = this.Direction.OriginHelpPoint;
            pocketHelpPoint.TransformByInverse(pocketTransform);

            pocketOperation.Direction = new BasicSmartDirection(pocketOperation, pocketDirection, pocketHelpPoint);

            //Set the section plane
            pocketOperation.SectionPlane = new BasicSmartPlane(null, new BoundedPlane(pocketOperation.Section.Geometry.Plane, Extent.Unit));

            //Set the side type and length
            pocketOperation.Side.Type = BoundType.Length;
            pocketOperation.Side.Length = this.Offset.Clone(null) as SmartReal;

            //Create the operation
            if (!pocketOperation.IsCreated)
            {
                pocketOperation.Parent = this;
                pocketOperation.Create(this.Owner);

                //Set the pocket operation as the operation of the femaleEntityOperationPair
                femaleEntityOperationPair.Operation = pocketOperation;
            }
            #endregion

            #region Boss Operation
            //Get the operation
            bossOperation = maleEntityOperationPair.Operation as BossOperation;

            //Create
            if (bossOperation == null)
            {
                bossOperation = new BossOperation(maleEntityOperationPair.PartEntity.GetDocumentHostingModificationOperations(), 0);
                bossOperation.IsDeletable = false;
                bossOperation.IsModifiable = false;
                bossOperation.IsMovable = false;
            }
            //Modify
            else
            {
                Document opDocument = bossOperation.Document;
                Document shEntitiesDocument = maleEntityOperationPair.PartEntity.GetDocumentHostingModificationOperations();

                if (opDocument != shEntitiesDocument)
                {
                    Operation.Delete(bossOperation);
                    bossOperation = new BossOperation(maleEntityOperationPair.PartEntity.GetDocumentHostingModificationOperations(), 0);
                    bossOperation.IsDeletable = false;
                    bossOperation.IsModifiable = false;
                    bossOperation.IsMovable = false;
                }
            }

            //Get male part entity transform
            Transform bossTransform = maleEntityOperationPair.PartEntity.OccurrenceDefinitionTransform;

            //Get the shape to modify
            List<ShapeEntity> bossShapeEntities = new List<ShapeEntity>();
            maleEntityOperationPair.PartEntity.GetShapesToModify(false, true, bossShapeEntities);
            bossOperation.ModifiedEntity = bossShapeEntities[0];

            //Transform and set the section
            TopSolid.Kernel.G.D3.Curves.IGeometricSection bossSection = this.Section.Geometry.Clone() as TopSolid.Kernel.G.D3.Curves.IGeometricSection;
            bossSection.TransformByInverse(bossTransform);
            bossOperation.Section = new BasicSmartSection(bossOperation, bossSection);

            //Transform and set the direction
            UnitVector bossDirection = this.Direction.Geometry;
            bossDirection.TransformByInverse(bossTransform);
            Point bossHelpPoint = this.Direction.OriginHelpPoint;
            bossHelpPoint.TransformByInverse(bossTransform);
            bossOperation.Direction = new BasicSmartDirection(bossOperation, bossDirection, bossHelpPoint);

            //Set the section plane
            bossOperation.SectionPlane = new BasicSmartPlane(bossOperation, new BoundedPlane(bossOperation.Section.Geometry.Plane, Extent.Unit));

            //Set the side type and length
            bossOperation.Side.Type = BoundType.Length;
            bossOperation.Side.Length = this.Offset.Clone(null) as SmartReal;

            //Create the operation
            if (!bossOperation.IsCreated)
            {
                bossOperation.Parent = this;
                bossOperation.Create(this.Owner);
                maleEntityOperationPair.Operation = bossOperation;
            }
            #endregion
        }



        // Interfaces:

        #region IWritable Members

        /// <summary>
        /// </summary>
        /// <param name="inWriter"></param>
        /// <inheritdoc />
        public override void Write(TopSolid.Kernel.SX.IO.IWriter inWriter)
        {
            base.Write(inWriter);

            this.femalePartHandle.Write(inWriter);
            this.malePartHandle.Write(inWriter);
            this.femaleEntityOperationPair.Write(inWriter);
            this.maleEntityOperationPair.Write(inWriter);
            this.offsetHandle.Write(inWriter);
            this.directionHandle.Write(inWriter);
            this.sectionHandle.Write(inWriter);
        }

        #endregion
    }

}
