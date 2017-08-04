﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace swbf
{
   class Modl_editor
   {
      static public void edit(Ucfb_editor model, uint factor)
      {
         var info = model.find_child("INFO");

         // SWBF and SWBFII have different sized INFO chunks
         // however the (what I assume is) poly count is always
         // the last field in these chunks.
         info.seek(-4, SeekOrigin.End, false);

         var current_count = info.read_uint32();
         info.seek(-4, SeekOrigin.Current, false);
         info.write_uint32(current_count / factor);

         while (!model.at_end)
         {
            var child = model.read_child();

            if (child.identifier == "shdw") edit_shadow(child, factor);
            else if (child.identifier == "segm") edit_segment(child, factor);
         }
      }

      static void edit_shadow(Ucfb_editor shadow, uint factor)
      {
         var gshd = shadow.find_child("GSHD");
         var info = gshd.find_child("INFO");
         
         info.seek(-8, SeekOrigin.End, false);

         var current_count = info.read_uint32();
         info.seek(-4, SeekOrigin.Current, false);
         info.write_uint32(current_count / factor);
      }

      static void edit_segment(Ucfb_editor segment, uint factor)
      {
         var info = segment.find_child("INFO");

         info.seek(-8, SeekOrigin.End, false);

         var current_count = info.read_uint32();
         info.seek(-4, SeekOrigin.Current, false);
         info.write_uint32(current_count / factor);
      }
   }
}