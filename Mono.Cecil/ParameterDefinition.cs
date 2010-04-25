//
// ParameterDefinition.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2010 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Mono.Collections.Generic;

namespace Mono.Cecil {

	public sealed class ParameterDefinition : ParameterReference, ICustomAttributeProvider, IConstantProvider, IMarshalInfoProvider {

		ushort attributes;

		internal IMethodSignature method;

		object constant = Mixin.NotResolved;
		Collection<CustomAttribute> custom_attributes;
		MarshalInfo marshal_info;

		public ParameterAttributes Attributes {
			get { return (ParameterAttributes) attributes; }
			set { attributes = (ushort) value; }
		}

		public IMethodSignature Method {
			get { return method; }
		}

		public bool HasConstant {
			get {
				ResolveConstant ();

				return constant != Mixin.NoValue;
			}
			set { if (!value) constant = Mixin.NoValue; }
		}

		public object Constant {
			get { return HasConstant ? constant : null;	}
			set { constant = value; }
		}

		void ResolveConstant ()
		{
			if (constant != Mixin.NotResolved)
				return;

			this.ResolveConstant (ref constant, parameter_type.Module);
		}

		internal bool HasImage {
			get { return parameter_type != null && parameter_type.Module != null && parameter_type.Module.HasImage; }
		}

		public bool HasCustomAttributes {
			get {
				if (custom_attributes != null)
					return custom_attributes.Count > 0;

				return this.GetHasCustomAttributes (parameter_type.Module);
			}
		}

		public Collection<CustomAttribute> CustomAttributes {
			get {
				if (custom_attributes != null)
					return custom_attributes;

				return custom_attributes = this.GetCustomAttributes (parameter_type.Module);
			}
		}

		public bool HasMarshalInfo {
			get {
				if (marshal_info != null)
					return true;

				return this.GetHasMarshalInfo (parameter_type.Module);
			}
		}

		public MarshalInfo MarshalInfo {
			get {
				if (marshal_info != null)
					return marshal_info;

				return marshal_info = this.GetMarshalInfo (parameter_type.Module);
			}
			set { marshal_info = value; }
		}

		#region ParameterAttributes

		public bool IsIn {
			get { return attributes.GetAttributes ((ushort) ParameterAttributes.In); }
			set { attributes = attributes.SetAttributes ((ushort) ParameterAttributes.In, value); }
		}

		public bool IsOut {
			get { return attributes.GetAttributes ((ushort) ParameterAttributes.Out); }
			set { attributes = attributes.SetAttributes ((ushort) ParameterAttributes.Out, value); }
		}

		public bool IsOptional {
			get { return attributes.GetAttributes ((ushort) ParameterAttributes.Optional); }
			set { attributes = attributes.SetAttributes ((ushort) ParameterAttributes.Optional, value); }
		}

		public bool HasDefault {
			get { return attributes.GetAttributes ((ushort) ParameterAttributes.HasDefault); }
			set { attributes = attributes.SetAttributes ((ushort) ParameterAttributes.HasDefault, value); }
		}

		#endregion

		public ParameterDefinition (TypeReference parameterType)
			: this (string.Empty, ParameterAttributes.None, parameterType)
		{
		}

		public ParameterDefinition (string name, ParameterAttributes attributes, TypeReference parameterType)
			: base (name, parameterType)
		{
			this.attributes = (ushort) attributes;
			this.token = new MetadataToken (TokenType.Param);
		}

		public override ParameterDefinition Resolve ()
		{
			return this;
		}
	}
}
