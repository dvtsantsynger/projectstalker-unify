using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace UniDownloader {
    public interface IDownloadService {
        IObservable<AssetBundle> GetAssetBundle(string path);

        IObservable<Texture2D> GetImage(string path);

        IObservable<string> GetText(string path);
    }

    public class DownloadService : MonoBehaviour, IDownloadService {

        [SerializeField]
        private string host = "https://mobile-assets.happify.com/empathy";

        public IObservable<AssetBundle> GetAssetBundle(string path) {
            return Observable.FromCoroutine<AssetBundle>((observer, cancellationToken) 
                => DownloadAssetBundle(new Uri(host + Platform() + path), observer, cancellationToken));
        }

        public IObservable<Texture2D> GetImage(string path) {
            return Observable.FromCoroutine<Texture2D>((observer, cancellationToken) 
                => DownloadImage(new Uri(host + Platform() + path), observer, cancellationToken));
        }

        public IObservable<string> GetText(string path) {
            return Observable.FromCoroutine<string>((observer, cancellationToken) 
                => DownloadText(new Uri(host + Platform() + path), observer, cancellationToken));
        }

        private string Platform() {
            switch(Application.platform) {
                case RuntimePlatform.Android:
                    return "/android";
                case RuntimePlatform.IPhonePlayer:
                    return "/ios";
                case RuntimePlatform.WebGLPlayer:
                    return "/web";
                default:
                    return "/standalone";
            }
        }

        #region Impl

        private static IEnumerator DownloadAssetBundle(Uri uri,
                                                       IObserver<AssetBundle> observer,
                                                       CancellationToken cancellationToken) {
            UnityWebRequest headReq = UnityWebRequest.Head(uri);
            yield return headReq.SendWebRequest();
            Dictionary<string, string> headers = headReq.GetResponseHeaders();
            string lastModification = headReq.GetResponseHeader("Last-Modified");
            uint version = (lastModification == null) ? 0 : (uint)lastModification.GetHashCode();
            UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(uri, version, 0);
            request.SendWebRequest();

            while (!request.isDone && !cancellationToken.IsCancellationRequested) {
                yield return null;
            }

            if (cancellationToken.IsCancellationRequested) yield break;

            if (request.error != null) {
                observer.OnError(new Exception(request.error));
            } else {
                observer.OnNext(DownloadHandlerAssetBundle.GetContent(request));
                observer.OnCompleted();
            }
        }

        private static IEnumerator DownloadImage(Uri uri,
                                                 IObserver<Texture2D> observer,
                                                 CancellationToken cancellationToken) {
            string path = uri.LocalPath;
            string fileName = Application.persistentDataPath + "/" + path.Replace('/', '_');
            FileInfo fileInfo = new FileInfo(fileName);

            UnityWebRequest headReq = UnityWebRequest.Head(uri);
            yield return headReq.SendWebRequest();

            if (headReq.error != null) {
                if (fileInfo.Exists) {
                    byte[] byteArray = File.ReadAllBytes(fileName);
                    Texture2D stexture = new Texture2D(1,1);
                    bool isLoaded = stexture.LoadImage(byteArray);
                    observer.OnNext(stexture);
                    observer.OnCompleted();
                } else {
                    observer.OnNext(null);
                    observer.OnCompleted();
                }
            } else {
                string lastModification = headReq.GetResponseHeader("Last-Modified");
                DateTime dt = DateTime.Parse(lastModification);
                if (!fileInfo.Exists || dt > fileInfo.LastWriteTimeUtc) {
                    UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri);
                    request.SendWebRequest();

                    while (!request.isDone && !cancellationToken.IsCancellationRequested) {
                        yield return null;
                    }

                    if (cancellationToken.IsCancellationRequested) yield break;

                    if (request.error != null) {
                        observer.OnError(new Exception(request.error));
                    } else {
                        Debug.Log("[Write file] " + fileName);
                        File.WriteAllBytes(fileName, request.downloadHandler.data);
                        observer.OnNext(DownloadHandlerTexture.GetContent(request));
                        observer.OnCompleted();
                    }
                } else {
                    if (fileInfo.Exists) {
                        byte[] byteArray = File.ReadAllBytes(fileName);
                        Texture2D stexture = new Texture2D(1,1);
                        bool isLoaded = stexture.LoadImage(byteArray);
                        observer.OnNext(stexture);
                        observer.OnCompleted();
                    } else {
                        observer.OnNext(null);
                        observer.OnCompleted();
                    }
                }
            }
        }

        private static IEnumerator DownloadText(Uri uri,
                                                 IObserver<string> observer,
                                                 CancellationToken cancellationToken) {
            string path = uri.LocalPath;
            string fileName = Application.persistentDataPath + "/" + path.Replace('/', '_');
            FileInfo fileInfo = new FileInfo(fileName);

            UnityWebRequest headReq = UnityWebRequest.Head(uri);
            yield return headReq.SendWebRequest();

            if (headReq.error != null) {
                if (fileInfo.Exists) {
                    string text = File.ReadAllText(fileName);
                    observer.OnNext(text);
                    observer.OnCompleted();
                } else {
                    observer.OnNext(null);
                    observer.OnCompleted();
                }
            } else {
                string lastModification = headReq.GetResponseHeader("Last-Modified");
                DateTime dt = DateTime.Parse(lastModification);
                if (!fileInfo.Exists || dt > fileInfo.LastWriteTimeUtc) {
                    UnityWebRequest request = UnityWebRequestTexture.GetTexture(uri);
                    request.SendWebRequest();

                    while (!request.isDone && !cancellationToken.IsCancellationRequested) {
                        yield return null;
                    }

                    if (cancellationToken.IsCancellationRequested) yield break;

                    if (request.error != null) {
                        observer.OnError(new Exception(request.error));
                    } else {
                        Debug.Log("[Write file] " + fileName);
                        File.WriteAllBytes(fileName, request.downloadHandler.data);
                        observer.OnNext(request.downloadHandler.text);
                        observer.OnCompleted();
                    }
                } else {
                    if (fileInfo.Exists) {
                         string text = File.ReadAllText(fileName);
                         observer.OnNext(text);
                        observer.OnCompleted();
                    } else {
                        observer.OnNext(null);
                        observer.OnCompleted();
                    }
                }
            }
        }
        #endregion
    }
}
