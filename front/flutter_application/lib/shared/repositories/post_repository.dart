import 'dart:convert';
import 'dart:developer';
import 'dart:io';

import 'package:flutter_application/shared/models/post_model.dart';
import 'package:http/http.dart' as http;
import 'package:http/io_client.dart';
import 'package:shared_preferences/shared_preferences.dart';

class PostRepository {
  final url = Uri.parse("https://10.0.2.2:7101/api/Post");
  late String token;
  final HttpClient httpClient = HttpClient()
    ..badCertificateCallback =
        (X509Certificate cert, String host, int port) => true; // Ignorar SSL

  Future<String> getTokenFromSharedPreferences() async {
    final prefs = await SharedPreferences.getInstance();
    token = prefs.getString('auth_token').toString();
    print("token$token");
    return token;
  }

  Future<Post> createPost(Post post) async {
    final ioClient = IOClient(httpClient);
    final response = await ioClient.post(url,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $token'
        },
        body: json.encode({'title': post.title, 'content': post.content}));
    log(response.statusCode.toString());
    print(response.statusCode.toString());
    if (response.statusCode == 201) {
      log("o post foi criado ${response.statusCode}");
      var json = jsonDecode(response.body);
      var test = Post.fromJson(json['data']);
      return test;
    } else {
      throw Exception("Erro ao criar o post");
    }
  }

  Future findById(String id) async {
    final url = Uri.parse("https://10.0.2.2:7101/api/Post/$id");
    final ioClient = IOClient(httpClient);
    final response = await ioClient.get(url, headers: {
      'Authorization': 'Bearer $token',
    });
    log(response.statusCode.toString());
    if (response.statusCode == 200) {
      var json = jsonDecode(response.body);
      Post post = Post.fromJson(json['data']);
      log(json['message']);
      log(post.title);
      log(post.id);
      log(response.statusCode.toString());
      return post;
    } else {
      log('Erro ao deletar  post: ${response.statusCode}');
      throw Exception('Erro ao deletar post');
    }
  }

  Future<List<Post>> findAll() async {
    final ioClient = IOClient(httpClient);
    await getTokenFromSharedPreferences();
    print("token$token");
    final response = await ioClient.get(url, headers: {
      'Authorization': 'Bearer $token',
    });
    if (response.statusCode == 200) {
      log('status  findALL(): ${response.statusCode}');
      final List<Post> posts = [];
      final body = jsonDecode(response.body);
      body['data'].map((item) {
        final Post post = Post.fromJson(item);
        posts.add(post);
      }).toList();
      return posts;
    } else {
      log('Erro ao obter posts: ${response.statusCode}');
      throw Exception('Erro ao buscar posts ${response.statusCode}');
    }
  }

  Future update(Post post) async {
    final url = Uri.parse("https://10.0.2.2:7101/api/Post/${post.id}");
    final ioClient = IOClient(httpClient);
    final response = await ioClient.put(url,
        headers: {
          'Content-Type': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: json.encode({'title': post.title, 'content': post.content}));
    log(response.statusCode.toString());
    if (response.statusCode == 200) {
      log('post editado com sucesso');
      log(response.statusCode.toString());
    } else {
      log('Erro ao atualizar  post: ${response.statusCode}');
      throw Exception('Erro ao atualizar post');
    }
  }

  Future delete(String id) async {
    final url = Uri.parse("https://10.0.2.2:7101/api/Post/$id");
    final ioClient = IOClient(httpClient);
    await getTokenFromSharedPreferences();
    final response = await ioClient.delete(url, headers: {
      'Authorization': 'Bearer $token',
    });
    log(response.statusCode.toString());
    if (response.statusCode == 204) {
      log('post deletado com sucesso');
      log(response.statusCode.toString());
    } else {
      log('Erro ao deletar  post: ${response.statusCode}');
      throw Exception('Erro ao deletar post');
    }
  }
}
