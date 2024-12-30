

import 'package:flutter_application/shared/models/post_model.dart';

class PostResponse {
  bool? success;
  int? status;
  String? timestamp;
  String? message;
  List<Post>? posts;
  Null error;

  PostResponse(
      {this.success,
      this.status,
      this.timestamp,
      this.message,
      this.posts,
      this.error});

  factory PostResponse.fromJson(Map<String, dynamic> json) {
    return PostResponse(
        success: json['success'],
        status: json['status'],
        timestamp: json['timestamp'],
        message: json['message'],
        posts: List<Post>.from(json['data']as List),
        error : json['error']);
  }

  Map toJson() {
     return {
        success: "",
        status:"",
        timestamp:"",
        message: "",
        posts:""
        };
        
    }
  
}
