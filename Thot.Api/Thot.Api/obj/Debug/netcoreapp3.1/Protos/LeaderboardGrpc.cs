// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/leaderboard.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Thot.Api {
  public static partial class Leaderboard
  {
    static readonly string __ServiceName = "leaderboard.Leaderboard";

    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    static readonly grpc::Marshaller<global::Thot.Api.LeaderboardRequest> __Marshaller_leaderboard_LeaderboardRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Thot.Api.LeaderboardRequest.Parser));
    static readonly grpc::Marshaller<global::Thot.Api.LeaderboardResponse> __Marshaller_leaderboard_LeaderboardResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::Thot.Api.LeaderboardResponse.Parser));

    static readonly grpc::Method<global::Thot.Api.LeaderboardRequest, global::Thot.Api.LeaderboardResponse> __Method_Top = new grpc::Method<global::Thot.Api.LeaderboardRequest, global::Thot.Api.LeaderboardResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Top",
        __Marshaller_leaderboard_LeaderboardRequest,
        __Marshaller_leaderboard_LeaderboardResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Thot.Api.LeaderboardReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Leaderboard</summary>
    [grpc::BindServiceMethod(typeof(Leaderboard), "BindService")]
    public abstract partial class LeaderboardBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Thot.Api.LeaderboardResponse> Top(global::Thot.Api.LeaderboardRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(LeaderboardBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Top, serviceImpl.Top).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, LeaderboardBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Top, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Thot.Api.LeaderboardRequest, global::Thot.Api.LeaderboardResponse>(serviceImpl.Top));
    }

  }
}
#endregion
