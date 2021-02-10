﻿using Android.OS;
using Android.Print;
using Android.Runtime;
using Java.Interop;
using Java.Lang;
using System;
using System.Collections.Generic;
using System.Text;
using Java.IO;
using Kit.Enums;
using Kit.Services;
using static Kit.Services.PDFToHtml;

namespace Kit.Droid.Services.HtmlToPDF
{
    [Register("android/print/PdfLayoutResultCallback")]
    public class PdfLayoutResultCallback : PrintDocumentAdapter.LayoutResultCallback
    {
        public PrintDocumentAdapter Adapter { get; set; }

        public PDFToHtml PDFToHtml { get; set; }

        public PdfLayoutResultCallback(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public PdfLayoutResultCallback() : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            if (!(Handle != IntPtr.Zero))
            {
                unsafe
                {
                    JniObjectReference val = JniPeerMembers.InstanceMethods.StartCreateInstance("()V", GetType(), null);
                    SetHandle(val.Handle, JniHandleOwnership.TransferLocalRef);
                    JniPeerMembers.InstanceMethods.FinishCreateInstance("()V", this, null);
                }
            }

        }

        public override void OnLayoutFinished(PrintDocumentInfo info, bool changed)
        {
            try
            {
                File file = new Java.IO.File(PDFToHtml.FilePath);
                ParcelFileDescriptor? fileDescriptor = ParcelFileDescriptor.Open(file, ParcelFileMode.ReadWrite);
                PdfWriteResultCallback writeResultCallback = new PdfWriteResultCallback(PDFToHtml);
                Adapter.OnWrite(new PageRange[] { PageRange.AllPages }, fileDescriptor, new CancellationSignal(), writeResultCallback);
            }
            catch
            {
                PDFToHtml.Status = PDFEnum.Failed;
            }

            base.OnLayoutFinished(info, changed);
        }

        public override void OnLayoutCancelled()
        {
            base.OnLayoutCancelled();
            PDFToHtml.Status = PDFEnum.Failed;
        }

        public override void OnLayoutFailed(ICharSequence error)
        {
            base.OnLayoutFailed(error);
            PDFToHtml.Status = PDFEnum.Failed;
        }
    }

    [Register("android/print/PdfWriteResult")]
    public class PdfWriteResultCallback : PrintDocumentAdapter.WriteResultCallback
    {
        readonly PDFToHtml pDFToHtml;

        public PdfWriteResultCallback(PDFToHtml _pDFToHtml, IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            pDFToHtml = _pDFToHtml;
        }

        public PdfWriteResultCallback(PDFToHtml _pDFToHtml) : base(IntPtr.Zero, JniHandleOwnership.DoNotTransfer)
        {
            if (!(Handle != IntPtr.Zero))
            {
                unsafe
                {
                    JniObjectReference val = JniPeerMembers.InstanceMethods.StartCreateInstance("()V", GetType(), null);
                    SetHandle(val.Handle, JniHandleOwnership.TransferLocalRef);
                    JniPeerMembers.InstanceMethods.FinishCreateInstance("()V", this, null);
                }
            }

            pDFToHtml = _pDFToHtml;
        }


        public override void OnWriteFinished(PageRange[] pages)
        {
            base.OnWriteFinished(pages);
            pDFToHtml.Status = PDFEnum.Completed;
        }

        public override void OnWriteCancelled()
        {
            base.OnWriteCancelled();
            pDFToHtml.Status = PDFEnum.Failed;
        }

        public override void OnWriteFailed(ICharSequence error)
        {
            base.OnWriteFailed(error);
            pDFToHtml.Status = PDFEnum.Failed;
        }
    }
}
