using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopSolid.Kernel.DB.Documents;
using TopSolid.Kernel.DB.Elements;
using TopSolid.Kernel.DB.Entities;
using TopSolid.Kernel.DB.Parameters;

namespace EPFL.SpeckleTopSolid.UI
{
    public class SpeckleStream : Entity
    {
        /* class StreamsFolderEntity : FolderEntity
         {
             public override Element Clone(Document inDocument, int inId)
             {
                 throw new NotImplementedException();
             }
         }
        */

        public string StreamID { get; set; }

        public SpeckleStream (Document inDocument, int inId) : base (inDocument, inId)
        {

        }

        public override Element Clone(Document inDocument, int inId)
        {
            throw new NotImplementedException();
        }


    }
}
