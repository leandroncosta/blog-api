import 'package:flutter/material.dart';
import 'package:flutter_application/shared/models/post_model.dart';
import 'package:flutter_application/shared/repositories/post_repository.dart';

class PostController {
  List<Post> posts = [];
  final PostRepository _postRepository;

  PostController([PostRepository? repository])
      : _postRepository = repository ?? PostRepository();

  final state = ValueNotifier<EPostState>(EPostState.start);
  Future start() async {
    state.value = EPostState.loading;
    try {
      posts = await _postRepository.findAll();
      state.value = EPostState.success;
    } catch (e) {
      state.value = EPostState.error;
      rethrow;
    }
  }

  Future findAll() async {
    state.value = EPostState.loading;
    try {
      posts = await _postRepository.findAll();
      state.value = EPostState.success;
    } catch (e) {
      state.value = EPostState.error;
      print("ERRO ao buscar posts $e");
      //rethrow;
    }
  }

  Future createPost(Post post) async {
    try {
      await _postRepository.createPost(post);
      await findAll();
    } catch (e) {
      print("ERRO ao criar post$e");
    }
  }

  Future delete(String id) async {
    //state.value = EPostState.loading;
    try {
      print("Deletando$id");
      await _postRepository.delete(id);
      await findAll();
      //state.value = EPostState.success;
    } catch (e) {
      print("ERRO ao deletar posts $e");
      state.value = EPostState.error;
    }
  }

  Future update(Post post) async {
    state.value = EPostState.loading;
    try {
      await _postRepository.update(post);
      state.value = EPostState.success;
      await findAll();
      //state.value = EPostState.success;
    } catch (e) {
      print("ERRO ao atualizar post $e");
      state.value = EPostState.error;
    }
  }

  Future findById(id) async {
    print("buscando post deid ${id}");
    try {
      state.value = EPostState.success;
      return  await _postRepository.findById(id);
    } catch (e) {
      state.value = EPostState.error;
      print("ERRO ao buscar post $e");
    }
  }
}

enum EPostState { start, loading, success, error }
