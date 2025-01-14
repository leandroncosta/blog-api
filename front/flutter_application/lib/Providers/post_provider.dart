import 'dart:math';

import 'package:flutter/material.dart';
import 'package:flutter_application/shared/controllers/post_controller.dart';
import 'package:flutter_application/shared/models/post_model.dart';

class PostProvider extends ChangeNotifier {
  PostController postController = PostController();
  List<Post> posts = [];
  late Future<List<Post>> _futurePosts;
  Future<List<Post>> get futurePosts => _futurePosts;

  bool _isDarkTheme = false;
  get isDarktheme => _isDarkTheme;
  
  PostProvider() {
    startFuture();
  }
  Future<void> startFuture() async {
    try {
      _futurePosts =  postController.findAll();
      print("${_futurePosts} length  future  in provider ");
      notifyListeners();
    } catch (e) {
      print(e);
    }
  }

  Future createPost(Post post) async {
    try {
      await postController.createPost(post);
      await startFuture();
      print("${posts.length} criei ");
      notifyListeners();
    } catch (e) {
      print(e);
    }
  }

  Future update(Post post) async {
    try {
      await postController.update(post);
      await startFuture();
      print("${posts.length} update ");
      notifyListeners();
    } catch (e) {
      print(e);
    }
  }

  Future delete(String id) async {
    try {
      await postController.delete(id);
      await startFuture();
      notifyListeners();
    } catch (e) {
      print(e);
    }
  }

  changeTheme() {
    _isDarkTheme = !_isDarkTheme;
    notifyListeners();
  }
}
